using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// 普通僵尸的行动脚本
/// </summary>
public class CommonZombie : Monster, IDamageable
{
    enum State
    {
        Walk,
        Atk,
        Stun,
    }
    /// <summary>
    /// 一个对应普通魔物的效应处理器
    /// </summary>
    private CommonMonsterHandler handler;
    
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
        handler = new CommonMonsterHandler(Data);

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

    /// <summary>
    /// 走路
    /// </summary>
    void Walk()
    {
        state = State.Walk;
        rigid.velocity = Vector2.left * Data.Speed / 100;
        animator.speed = Data.Speed / 50f;
    }

    Coroutine atkCoroutine;
    //攻击协程
    IEnumerator AtkCoroutine(ICharactorData charactor)
    {
        state = State.Atk;
        
        float atkDistanceSeconds = 0.5f;
        rigid.velocity = Vector2.zero;
        animator.SetBool("IsAttack", true);

        while(charactor.Health > 0 && isAlive)
        {
            while (state == State.Stun)//处于眩晕状态 不进行处理
            {
                animator.SetBool("IsAttack", false);
                yield return 1;
            }
            yield return true;

            state = State.Atk;

            charactor.Health -= Data.AtkPower;
            yield return new WaitForSecondsRealtime(atkDistanceSeconds);
        }
        //停止攻击动画
        animator.SetBool("IsAttack", false);
        state = State.Walk;

        atkCoroutine = null;
    }
    /// <summary>
    /// 被眩晕
    /// </summary>
    void Stun()
    {
        state = State.Stun;
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
                    return new Color(1f, 0.25f, 0);
                case Elements.Ice:
                    return new Color(0, 1f, 1f);
                case Elements.Electric:
                    return new Color(0.75f, 0, 1f);
                //风和岩不会附着
                case Elements.Grass:
                    return Color.green;
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
        if(damageCoroutine == null)
            damageCoroutine = StartCoroutine(DamageAnimate());
    }
    
    /// <summary>
    /// 超出地图删除
    /// </summary>
    public void CheckLocation()
    {
        if (GameController.Instance.WorldToGrid(transform.position) == new Vector2(-1, -1))
            Destroy(gameObject);
    }

    public void Update()
    {
        //用普通魔物效果分析器来对Data里的所有效果进行分析
        handler.CheckEffect(Data.GetEffects());
        if (isAlive)
        {
            //查找身上有没有眩晕效果
            IEffect stunEffect = Data.GetEffects().Find((effect) => effect is StunEffect);
            if (stunEffect == null)
            {
                if(state != State.Atk)
                {
                    Walk();
                }
            }
            else
            {
                Stun();
            }

            CheckLocation();
            //计算颜色
            Color c = CalculateElementColor();
            if (damageCoroutine != null)//现在还在显示伤害特效的帧里
                c = (c + DamageColor) / 2;
            sprite.color = c;
        }
        if (Data.Health <= 0 && isAlive)//只有活着的时候才能死亡
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
        float secondsBeforeBodyDisappear = 2;
        Color elementColor = CalculateElementColor();//计算此时附着的颜色
        sprite.color = elementColor;
        FallingHead.GetComponent<SpriteRenderer>().color = elementColor ;//把掉的头设置成和自己一样的颜色
        animator.Play("Die");
        FallingHead.SetActive(true);
        yield return new WaitForSecondsRealtime(secondsBeforeBodyDisappear);

        Data.RemoveOnReceiveAllDamageListener(OnDamage);//移除监听
        Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        IMonsterAttackable attackable = collision.gameObject.GetComponent<IMonsterAttackable>();
        if (attackable != null && atkCoroutine == null)
        {
            ICharactorData data = attackable.GetData();
            //这个函数是在物理帧调用，而Data的赋值是在渲染帧调用，
            //刚放下一个植物的时候容易出现 触发器已经触发但是Data还未赋值的情况
            if (data != null)
                atkCoroutine = StartCoroutine(AtkCoroutine(data));
        }
    }
    public IDamageReceiver GetReceiver()
    {
        return Data;
    }
}
