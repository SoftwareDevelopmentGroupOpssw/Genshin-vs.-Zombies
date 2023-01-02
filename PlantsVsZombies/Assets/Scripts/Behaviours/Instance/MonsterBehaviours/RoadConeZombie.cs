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
    /// һ����Ӧ��ͨħ���ЧӦ������
    /// </summary>
    private DefaultHandler handler;

    private SpriteRenderer sprite;
    private Rigidbody2D rigid;
    private Collider2D colliders;
    private Animator animator;
    /// <summary>
    /// ���ڵ�״̬
    /// </summary>
    private State state;

    /// <summary>
    /// �Ƿ񻹴��
    /// </summary>
    private bool isAlive = true;

    /// <summary>
    /// ��������ͷ��������ʱ��������������
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

        //�������һ����·����
        System.Random walkStyle = new System.Random();
        animator.SetInteger("WalkStyle", walkStyle.Next(1, 4));
        state = State.Walk;

        Data.AddOnReceiveAllDamageListener(OnDamage);
    }
    bool TrySetState(State changeState)
    {
        if ((int)changeState > (int)state)//�л���״̬�����ȼ������ڸ�
        {
            state = changeState;
            return true;
        }
        else
            return false;
    }
    /// <summary>
    /// ��·
    /// </summary>
    void Walk()
    {
        rigid.velocity = Vector2.left * Data.Speed / 100f;
        animator.speed = Data.Speed / 15f;
    }

    Coroutine atkCoroutine;
    //����Э��
    IEnumerator AtkCoroutine(IDamageReceiver receiver)
    {
        float atkDistanceSeconds = 0.04f;
        rigid.velocity = Vector2.zero;

        do
        {
            while (state != State.Atk || this.enabled == false) //���ڹ���״̬���߽ű���ʧ����
            {
                if (TrySetState(State.Atk) && this.enabled == true)
                    animator.SetBool("IsAttack", true);
                yield return 1;
            }

            receiver.ReceiveDamage(new SystemDamage(Data.AtkPower, Elements.None));
            yield return new WaitForSecondsRealtime(atkDistanceSeconds);

        } while (receiver.Health > 0 && isAlive);
        //ֹͣ��������
        animator.SetBool("IsAttack", false);
        state = State.Idle;
        atkCoroutine = null;
    }
    /// <summary>
    /// ��ѣ��
    /// </summary>
    void Stun()
    {
        rigid.velocity = Vector2.zero;
        animator.speed = 0;//����ֹͣ
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
                    return new Color(0.6f, 1f, 0);
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
        if (damageCoroutine == null)
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

        if (isAlive)
        {
            //����ͨЧ������������Data�������Ч�����з���
            handler.CheckEffect();

            CheckLocation();

            //����������û��ѣ��Ч��
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
        if (Data is RoadConeZombieData && Data.Health <= 0 && isAlive) //·������ֵΪ0���л�����ͨ��ʬ��̬
        {
            animator.SetTrigger("Break");//·�ϵ���
            Data.RemoveOnReceiveAllDamageListener(OnDamage);//�Ƴ�����
            Data.GameObject = null;//ԭ����data

            IMonsterData newData = MonsterPrefabSerializer.Instance.GetMonsterData("CommonZombie");//��ȡһ���µ�data
            //����ԭ����Ԫ����Ϣ���µ�Data��
            Elements[] elements = Data.GetAllElements();
            foreach (Elements element in elements)
                newData.AddElement(element);
            //������� ����ԭ��������
            handler.DisableAll();
            Data.Dispose();
            Data = newData;
            Data.GameObject = gameObject;//����data����Ϸ�������ó��Լ�
            Data.AddOnReceiveAllDamageListener(OnDamage);//������Ӽ���
            handler = new DefaultHandler(newData);
        }
        else if(Data is CommonZombieData && Data.Health <= 0 && isAlive)//��������ֵΪ0,����
        {
            StopAllCoroutines();
            StartCoroutine(Die());
        }
    }
    /// <summary>
    /// ����ʱ�������߼�
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        isAlive = false;
        rigid.velocity = Vector2.zero;
        colliders.enabled = false;//�ر���ײ�У������Ͳ����赲�ӵ�

        Data.RemoveOnReceiveAllDamageListener(OnDamage);//�Ƴ�����
        handler.DisableAll();
        GameController.Instance.MonstersController.RemoveMonster(this);//��Data��������

        Color elementColor = CalculateElementColor();//�����ʱ���ŵ���ɫ
        sprite.color = elementColor;
        FallingHead.GetComponent<SpriteRenderer>().color = elementColor;//�ѵ���ͷ���óɺ��Լ�һ������ɫ
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
            //���������������֡���ã���Data�ĸ�ֵ������Ⱦ֡���ã�
            //�շ���һ��ֲ���ʱ�����׳��� �������Ѿ���������Data��δ��ֵ�����
            if (receiver != null)
                atkCoroutine = StartCoroutine(AtkCoroutine(receiver));
        }
    }
    public IDamageReceiver GetReceiver()
    {
        return Data;
    }
}
