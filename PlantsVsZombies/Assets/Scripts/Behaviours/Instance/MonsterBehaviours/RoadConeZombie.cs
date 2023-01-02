using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadConeZombie : Monster, IDamageable
{
    enum State
    {
        Idle,
        Walk,
        Atk,
        Stun,
    }
    /// <summary>
    /// 一个对应普通魔物的效应处理器
    /// </summary>
    private DefaultHandler handler;

    private SpriteRenderer sprite;
    private Rigidbody2D rigid;
    private Collider2D colliders;
    private Animator animator;
    /// <summary>
    /// 现在的状态
    /// </summary>
    private State state;

    /// <summary>
    /// 是否还存活
    /// </summary>
    private bool isAlive = true;

    /// <summary>
    /// 掉下来的头，死亡的时候会启用这个物体
    /// </summary>
    public GameObject FallingHead;

    public override IEffectHandler Handler => handler;

    private void Start()
    {
        handler = new DefaultHandler(Data);

        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        colliders = GetComponent<Collider2D>();

        //随机产生一个走路姿势
        System.Random walkStyle = new System.Random();
        animator.SetInteger("WalkStyle", walkStyle.Next(1, 4));
        state = State.Walk;

        Data.AddOnReceiveAllDamageListener(OnDamage);
    }
    bool TrySetState(State changeState)
    {
        if ((int)changeState > (int)state)//切换的状态的优先级比现在高
        {
            state = changeState;
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// 走路
    /// </summary>
    void Walk()
    {
        rigid.velocity = Vector2.left * Data.Speed / 100f;
        animator.speed = Data.Speed / 15f;
    }

    Coroutine atkCoroutine;
    //攻击协程
    IEnumerator AtkCoroutine(IDamageReceiver receiver)
    {
        float atkDistanceSeconds = 0.04f;
        rigid.velocity = Vector2.zero;

        do
        {
            while (state != State.Atk || this.enabled == false) //不在攻击状态或者脚本被失活了
            {
                if (TrySetState(State.Atk) && this.enabled == true)
                    animator.SetBool("IsAttack", true);
                yield return 1;
            }

            receiver.ReceiveDamage(new SystemDamage(Data.AtkPower, Elements.None));
            yield return new WaitForSecondsRealtime(atkDistanceSeconds);

        } while (receiver.Health > 0 && isAlive);
        //停止攻击动画
        animator.SetBool("IsAttack", false);
        state = State.Idle;
        atkCoroutine = null;
    }
    /// <summary>
    /// 被眩晕
    /// </summary>
    void Stun()
    {
        rigid.velocity = Vector2.zero;
        animator.speed = 0;//动画停止
    }

    #region 颜色特效计算
    /// <summary>
    /// 计算受到元素附着时的身上的颜色值(改变颜色)
    /// </summary>
    Color CalculateElementColor()
    {
        Color GetColor(Elements element)
        {
            switch (element)
            {
                case Elements.Water:
                    return new Color(0, 0.5f, 1f);
                case Elements.Fire:
                    return new Color(1f, 0.4f, 0.2f);
                case Elements.Ice:
                    return new Color(0, 1f, 1f);
                case Elements.Electric:
                    return new Color(0.75f, 0, 1f);
                //风和岩不会附着
                case Elements.Grass:
                    return new Color(0.6f, 1f, 0);
            }
            return Color.black;
        }
        Elements[] elements = Data.GetAllElements();
        Color color;
        if (elements.Length == 0)//没有元素附着，恢复到正常颜色
            color = Color.white;
        else if (elements.Length == 1)//只有一个元素附着
            color = GetColor(elements[0]);
        else//有多个元素附着
        {
            color = GetColor(elements[0]);
            for (int i = 1; i < elements.Length; i++)
            {
                color = (color + GetColor(elements[i])) / 2;
            }
        }
        return color;
    }
    /// <summary>
    /// 受到伤害时显示的颜色
    /// </summary>
    Color DamageColor = Color.red;
    /// <summary>
    /// 受到伤害时启用的协程
    /// </summary>
    private Coroutine damageCoroutine;

    /// <summary>
    /// 启动受到伤害特效协程
    /// </summary>
    /// <returns></returns>
    IEnumerator DamageAnimate()
    {
        float second = 0.1f;//变红持续的秒数
        yield return new WaitForSecondsRealtime(second);
        damageCoroutine = null;
    }
    #endregion

    /// <summary>
    /// 受到伤害时
    /// </summary>
    /// <param name="element"></param>
    void OnDamage(IElementalDamage element)
    {
        if (damageCoroutine == null)
            damageCoroutine = StartCoroutine(DamageAnimate());
    }

    /// <summary>
    /// 超出地图直接游戏结束
    /// </summary>
    public void CheckLocation()
    {
        if (GameController.Instance.WorldToGrid(transform.position) == new Vector2(-1, -1))
        {
            GameController.Instance.ShowResult(false);
            animator.enabled = true;//让这个超出地图的僵尸开启动画
        }
    }

    public void Update()
    {

        //计算颜色
        Color c = CalculateElementColor();
        if (damageCoroutine != null)//现在还在显示伤害特效的帧里
            c = (c + DamageColor) / 2;
        sprite.color = c;

        if (isAlive)
        {
            //用普通效果分析器来对Data里的所有效果进行分析
            handler.CheckEffect();

            CheckLocation();

            //查找身上有没有眩晕效果
            IContainedStunEffect stunEffect = Data.GetEffects().Find((effect) => effect is IContainedStunEffect) as IContainedStunEffect;
            if (stunEffect != null)
            {
                if (!stunEffect.IsStunEffectOver)
                    TrySetState(State.Stun);
                else
                    state = State.Idle;
            }
            else if (stunEffect == null && state == State.Stun)
            {
                state = State.Idle;
            }
            if (Data.Strength < 0)
            {
                TrySetState(State.Stun);
            }

            TrySetState(State.Walk);
            switch (state)
            {
                case State.Walk:
                    Walk();
                    break;
                case State.Stun:
                    Stun();
                    break;
            }

        }
        if (Data is RoadConeZombieData && Data.Health <= 0 && isAlive) //路障生命值为0，切换到普通僵尸形态
        {
            animator.SetTrigger("Break");//路障掉落
            Data.RemoveOnReceiveAllDamageListener(OnDamage);//移除监听
            Data.GameObject = null;//原来的data

            IMonsterData newData = MonsterPrefabSerializer.Instance.GetMonsterData("CommonZombie");//获取一个新的data
            //复制原来的元素信息到新的Data中
            Elements[] elements = Data.GetAllElements();
            foreach (Elements element in elements)
                newData.AddElement(element);
            //复制完成 丢弃原来的数据
            handler.DisableAll();
            Data.Dispose();
            Data = newData;
            Data.GameObject = gameObject;//将新data的游戏物体设置成自己
            Data.AddOnReceiveAllDamageListener(OnDamage);//重新添加监听
            handler = new DefaultHandler(newData);
        }
        else if(Data is CommonZombieData && Data.Health <= 0 && isAlive)//本体生命值为0,死亡
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }
    /// <summary>
    /// 死亡时触发的逻辑
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        isAlive = false;
        rigid.velocity = Vector2.zero;
        colliders.enabled = false;//关闭碰撞盒，这样就不会阻挡子弹

        Data.RemoveOnReceiveAllDamageListener(OnDamage);//移除监听
        handler.DisableAll();
        GameController.Instance.MonstersController.RemoveMonster(this);//将Data数据清理

        Color elementColor = CalculateElementColor();//计算此时附着的颜色
        sprite.color = elementColor;
        FallingHead.GetComponent<SpriteRenderer>().color = elementColor;//把掉的头设置成和自己一样的颜色
        animator.speed = Data.Speed / 15f;
        animator.Play("Die");
        FallingHead.SetActive(true);


        float secondsBeforeBodyDisappear = 1;
        yield return new WaitForSecondsRealtime(secondsBeforeBodyDisappear);
        Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null && atkCoroutine == null && damageable is Plant)
        {
            IDamageReceiver receiver = damageable.GetReceiver();
            //这个函数是在物理帧调用，而Data的赋值是在渲染帧调用，
            //刚放下一个植物的时候容易出现 触发器已经触发但是Data还未赋值的情况
            if (receiver != null)
                atkCoroutine = StartCoroutine(AtkCoroutine(receiver));
        }
    }
    public IDamageReceiver GetReceiver()
    {
        return Data;
    }
}
