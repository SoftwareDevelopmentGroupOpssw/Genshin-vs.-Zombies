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
    /// <summary>
    /// 一个对应普通魔物的效应处理器
    /// </summary>
    private DefaultHandler handler;
    public override IEffectHandler Handler => handler;

    private SpriteRenderer sprite;
    private Rigidbody2D rigid;
    private Collider2D colliders;
    private Animator animator;

    /// <summary>
    /// 掉下来的头，死亡的时候会启用这个物体
    /// </summary>
    public GameObject FallingHead;

    /// <summary>
    /// 设置一个状态机，默认状态：走路
    /// </summary>
    private StateMachine<MonsterState, StateKey, string> stateMachine;



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
        #region 构造状态机
        stateMachine = new StateMachine<MonsterState, StateKey, string>(StateKey.Walk, new Walk(this));
        Attack attack = new Attack(this);
        stateMachine.AddState(StateKey.Attack, attack, attack.OnEnterState, attack.OnExitState);
        stateMachine.AddState(StateKey.Stun, new Stun(this));
        Die die = new Die(this);
        stateMachine.AddState(StateKey.Die, die, die.OnEnterState, die.OnExitState);
        //可以攻击
        stateMachine.AddAction("CanAttack", 
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Walk, StateKey.Attack),
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Attack, StateKey.Attack) //更换目标攻击
            );
        //植物被吃完
        stateMachine.AddAction("PlantAte", new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Attack, StateKey.Walk));
        //被眩晕
        stateMachine.AddAction("Stunned",
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Walk, StateKey.Stun),
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Attack, StateKey.Stun),
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Stun, StateKey.Stun)
            );
        //从眩晕中回复
        stateMachine.AddAction("Recover", new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Stun, StateKey.Walk));
        stateMachine.AddAction("Die",
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Walk, StateKey.Die),
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Attack, StateKey.Die),
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Stun, StateKey.Die)
            );
        #endregion

        Data.AddOnReceiveAllDamageListener(OnDamage);
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
                    return new Color(0.6f,1f,0);
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
        if (stateMachine.CurrentKey != StateKey.Die)
        {
            if (Data.Health <= 0)
            {
                stateMachine.TriggerAction("Die");
            }
            else
            {
                //用普通效果分析器来对Data里的所有效果进行分析
                handler.CheckEffect();

                CheckLocation();


                //查找身上有没有眩晕效果
                IContainedStunEffect stunEffect = Data.GetEffects().Find((effect) =>
                {
                    if (effect is IContainedStunEffect)
                    {
                        return !(effect as IContainedStunEffect).IsStunEffectOver;
                    }
                    else
                        return false;
                }) as IContainedStunEffect;
                if (stunEffect != null || Data.Strength <= 0)
                {
                    stateMachine.TriggerAction("Stunned");
                }
                else if (stateMachine.CurrentKey == StateKey.Stun)
                {
                    stateMachine.TriggerAction("Recover");
                }
            }
            stateMachine.Current.Update();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null && damageable is Plant)
        {
            IDamageReceiver receiver = damageable.GetReceiver();
            //这个函数是在物理帧调用，而Data的赋值是在渲染帧调用，
            //刚放下一个植物的时候容易出现 触发器已经触发但是Data还未赋值的情况
            //receiver不为null且与正在攻击的目标不一样
            if (receiver != null && !receiver.Equals((stateMachine[StateKey.Attack] as Attack).ReceiverUnderAttack))
            {
                (stateMachine[StateKey.Attack] as Attack).ReceiverUnderAttack = receiver;
                stateMachine.TriggerAction("CanAttack");//触发攻击
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null && damageable is Plant)
        {
            IDamageReceiver receiver = damageable.GetReceiver();
            //现在存活且离开的目标与正在攻击的目标相同
            if (receiver != null && stateMachine.CurrentKey != StateKey.Die && 
                receiver.Equals((stateMachine[StateKey.Attack] as Attack).ReceiverUnderAttack))
            {
                stateMachine.TriggerAction("PlantAte");//植物被吃掉，回到走路状态
            }
        }
    }
    public IDamageReceiver GetReceiver()
    {
        return Data;
    }


    enum StateKey
    {
        Walk,
        Attack,
        Stun,
        Die,
    }
    class Walk : MonsterState //魔物的走路状态
    {
        private Rigidbody2D rigid;
        private Animator animator;
        private IMonsterData data;
        public Walk(CommonZombie target)
        {
            rigid = target.GetComponent<Rigidbody2D>();
            animator = target.GetComponent<Animator>();
            data = target.Data;
        }
        public override void Update()
        {
            rigid.velocity = Vector2.left * data.Speed / 100f;
            animator.speed = data.Speed / 15f;
        }
    }
    class Attack : MonsterState //魔物的攻击状态
    {
        private Rigidbody2D rigid;
        private Animator animator;
        private CommonZombie target;
        private Coroutine attackCoroutine;
        /// <summary>
        /// 受到攻击的目标
        /// </summary>
        public IDamageReceiver ReceiverUnderAttack { get; set; }
        public Attack(CommonZombie target)
        {
            this.target = target;
            rigid = target.GetComponent<Rigidbody2D>();
            animator = target.GetComponent<Animator>();
        }
        IEnumerator AttackCoroutine()
        {
            float atkDistanceSeconds = 0.04f;
            while(true)//处于攻击状态，则一直尝试进行攻击
            {
                while (target.enabled == false) //脚本被失活了
                    yield return 1;
                while (ReceiverUnderAttack == null)//被攻击的对象为空，等待物理帧传入一个新的对象来攻击
                    yield return 1;
                ReceiverUnderAttack.ReceiveDamage(new SystemDamage(target.Data.AtkPower, Elements.None));
                yield return new WaitForSecondsRealtime(atkDistanceSeconds);
            }
        }
        public override void OnEnterState()
        {
            attackCoroutine = target.StartCoroutine(AttackCoroutine());
            rigid.velocity = Vector2.zero;
            animator.SetBool("IsAttack", true);
        }
        public override void OnExitState()
        {
            target.StopCoroutine(attackCoroutine);
            animator.SetBool("IsAttack", false);
        }
    }
    class Stun : MonsterState //魔物的眩晕状态
    {
        private Rigidbody2D rigid;
        private Animator animator;
        public Stun(CommonZombie target)
        {
            rigid = target.GetComponent<Rigidbody2D>();
            animator = target.GetComponent<Animator>();
        }
        public override void Update()
        {
            rigid.velocity = Vector2.zero;
            animator.speed = 0;//动画停止
        }
    }
    class Die : MonsterState //魔物的死亡状态
    {
        private CommonZombie target;
        public Die(CommonZombie target)
        {
            this.target = target;
        }
        public override void OnEnterState()
        {
            IEnumerator DelayDestroy()
            {
                float secondsBeforeBodyDisappear = 1;
                yield return new WaitForSecondsRealtime(secondsBeforeBodyDisappear);
                GameController.Instance.MonstersController.RemoveMonster(target);//将Data数据清理
                Destroy(target.gameObject);
            }
            Rigidbody2D rigid = target.GetComponent<Rigidbody2D>();
            Collider2D colliders = target.GetComponent<Collider2D>();
            Animator animator = target.GetComponent<Animator>();
            SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();

            rigid.velocity = Vector2.zero;
            colliders.enabled = false;//关闭碰撞盒，这样就不会阻挡子弹

            target.Data.RemoveOnReceiveAllDamageListener(target.OnDamage);//移除监听
            target.handler.DisableAll();

            Color elementColor = target.CalculateElementColor();//计算此时附着的颜色
            sprite.color = elementColor;
            target.FallingHead.GetComponent<SpriteRenderer>().color = elementColor;//把掉的头设置成和自己一样的颜色
            animator.speed = target.Data.Speed / 15f;
            animator.Play("Die");
            target.FallingHead.SetActive(true);
            target.StartCoroutine(DelayDestroy());
        }
    }
}
