using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>
/// ��ͨ��ʬ���ж��ű�
/// </summary>
public class CommonZombie : Monster, IDamageable
{
    /// <summary>
    /// һ����Ӧ��ͨħ���ЧӦ������
    /// </summary>
    private DefaultHandler handler;
    public override IEffectHandler Handler => handler;

    private SpriteRenderer sprite;
    private Rigidbody2D rigid;
    private Collider2D colliders;
    private Animator animator;

    /// <summary>
    /// ��������ͷ��������ʱ��������������
    /// </summary>
    public GameObject FallingHead;

    /// <summary>
    /// ����һ��״̬����Ĭ��״̬����·
    /// </summary>
    private StateMachine<MonsterState, StateKey, string> stateMachine;



    private void Start()
    {
        handler = new DefaultHandler(Data);

        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        colliders = GetComponent<Collider2D>();

        //�������һ����·����
        System.Random walkStyle = new System.Random();
        animator.SetInteger("WalkStyle", walkStyle.Next(1, 4));
        #region ����״̬��
        stateMachine = new StateMachine<MonsterState, StateKey, string>(StateKey.Walk, new Walk(this));
        Attack attack = new Attack(this);
        stateMachine.AddState(StateKey.Attack, attack, attack.OnEnterState, attack.OnExitState);
        stateMachine.AddState(StateKey.Stun, new Stun(this));
        Die die = new Die(this);
        stateMachine.AddState(StateKey.Die, die, die.OnEnterState, die.OnExitState);
        //���Թ���
        stateMachine.AddAction("CanAttack", 
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Walk, StateKey.Attack),
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Attack, StateKey.Attack) //����Ŀ�깥��
            );
        //ֲ�ﱻ����
        stateMachine.AddAction("PlantAte", new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Attack, StateKey.Walk));
        //��ѣ��
        stateMachine.AddAction("Stunned",
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Walk, StateKey.Stun),
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Attack, StateKey.Stun),
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Stun, StateKey.Stun)
            );
        //��ѣ���лظ�
        stateMachine.AddAction("Recover", new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Stun, StateKey.Walk));
        stateMachine.AddAction("Die",
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Walk, StateKey.Die),
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Attack, StateKey.Die),
            new StateMachine<MonsterState, StateKey, string>.Action(StateKey.Stun, StateKey.Die)
            );
        #endregion

        Data.AddOnReceiveAllDamageListener(OnDamage);
    }

    #region ��ɫ��Ч����
    /// <summary>
    /// �����ܵ�Ԫ�ظ���ʱ�����ϵ���ɫֵ(�ı���ɫ)
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
                //����Ҳ��ḽ��
                case Elements.Grass:
                    return new Color(0.6f,1f,0);
            }
            return Color.black;
        }
        Elements[] elements = Data.GetAllElements();
        Color color;
        if (elements.Length == 0)//û��Ԫ�ظ��ţ��ָ���������ɫ
            color = Color.white;
        else if (elements.Length == 1)//ֻ��һ��Ԫ�ظ���
            color = GetColor(elements[0]);
        else//�ж��Ԫ�ظ���
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
    /// �ܵ��˺�ʱ��ʾ����ɫ
    /// </summary>
    Color DamageColor = Color.red;
    /// <summary>
    /// �ܵ��˺�ʱ���õ�Э��
    /// </summary>
    private Coroutine damageCoroutine;
    
    /// <summary>
    /// �����ܵ��˺���ЧЭ��
    /// </summary>
    /// <returns></returns>
    IEnumerator DamageAnimate()
    {
        float second = 0.1f;//������������
        yield return new WaitForSecondsRealtime(second);
        damageCoroutine = null;
    }
    #endregion

    /// <summary>
    /// �ܵ��˺�ʱ
    /// </summary>
    /// <param name="element"></param>
    void OnDamage(IElementalDamage element)
    {
        if(damageCoroutine == null)
            damageCoroutine = StartCoroutine(DamageAnimate());
    }
    
    /// <summary>
    /// ������ͼֱ����Ϸ����
    /// </summary>
    public void CheckLocation()
    {
        if (GameController.Instance.WorldToGrid(transform.position) == new Vector2(-1, -1))
        {
            GameController.Instance.ShowResult(false);
            animator.enabled = true;//�����������ͼ�Ľ�ʬ��������
        }
    }

    public void Update()
    {
        //������ɫ
        Color c = CalculateElementColor();
        if (damageCoroutine != null)//���ڻ�����ʾ�˺���Ч��֡��
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
                //����ͨЧ������������Data�������Ч�����з���
                handler.CheckEffect();

                CheckLocation();


                //����������û��ѣ��Ч��
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
            //���������������֡���ã���Data�ĸ�ֵ������Ⱦ֡���ã�
            //�շ���һ��ֲ���ʱ�����׳��� �������Ѿ���������Data��δ��ֵ�����
            //receiver��Ϊnull�������ڹ�����Ŀ�겻һ��
            if (receiver != null && !receiver.Equals((stateMachine[StateKey.Attack] as Attack).ReceiverUnderAttack))
            {
                (stateMachine[StateKey.Attack] as Attack).ReceiverUnderAttack = receiver;
                stateMachine.TriggerAction("CanAttack");//��������
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if (damageable != null && damageable is Plant)
        {
            IDamageReceiver receiver = damageable.GetReceiver();
            //���ڴ�����뿪��Ŀ�������ڹ�����Ŀ����ͬ
            if (receiver != null && stateMachine.CurrentKey != StateKey.Die && 
                receiver.Equals((stateMachine[StateKey.Attack] as Attack).ReceiverUnderAttack))
            {
                stateMachine.TriggerAction("PlantAte");//ֲ�ﱻ�Ե����ص���·״̬
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
    class Walk : MonsterState //ħ�����·״̬
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
    class Attack : MonsterState //ħ��Ĺ���״̬
    {
        private Rigidbody2D rigid;
        private Animator animator;
        private CommonZombie target;
        private Coroutine attackCoroutine;
        /// <summary>
        /// �ܵ�������Ŀ��
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
            while(true)//���ڹ���״̬����һֱ���Խ��й���
            {
                while (target.enabled == false) //�ű���ʧ����
                    yield return 1;
                while (ReceiverUnderAttack == null)//�������Ķ���Ϊ�գ��ȴ�����֡����һ���µĶ���������
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
    class Stun : MonsterState //ħ���ѣ��״̬
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
            animator.speed = 0;//����ֹͣ
        }
    }
    class Die : MonsterState //ħ�������״̬
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
                GameController.Instance.MonstersController.RemoveMonster(target);//��Data��������
                Destroy(target.gameObject);
            }
            Rigidbody2D rigid = target.GetComponent<Rigidbody2D>();
            Collider2D colliders = target.GetComponent<Collider2D>();
            Animator animator = target.GetComponent<Animator>();
            SpriteRenderer sprite = target.GetComponent<SpriteRenderer>();

            rigid.velocity = Vector2.zero;
            colliders.enabled = false;//�ر���ײ�У������Ͳ����赲�ӵ�

            target.Data.RemoveOnReceiveAllDamageListener(target.OnDamage);//�Ƴ�����
            target.handler.DisableAll();

            Color elementColor = target.CalculateElementColor();//�����ʱ���ŵ���ɫ
            sprite.color = elementColor;
            target.FallingHead.GetComponent<SpriteRenderer>().color = elementColor;//�ѵ���ͷ���óɺ��Լ�һ������ɫ
            animator.speed = target.Data.Speed / 15f;
            animator.Play("Die");
            target.FallingHead.SetActive(true);
            target.StartCoroutine(DelayDestroy());
        }
    }
}
