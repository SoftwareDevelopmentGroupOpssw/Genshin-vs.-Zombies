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
    enum State
    {
        Walk,
        Atk,
        Stun,
    }
    /// <summary>
    /// һ����Ӧ��ͨħ���ЧӦ������
    /// </summary>
    private CommonMonsterHandler handler;
    
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
        handler = new CommonMonsterHandler(Data);

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

    /// <summary>
    /// ��·
    /// </summary>
    void Walk()
    {
        state = State.Walk;
        rigid.velocity = Vector2.left * Data.Speed / 100;
        animator.speed = Data.Speed / 50f;
    }

    Coroutine atkCoroutine;
    //����Э��
    IEnumerator AtkCoroutine(ICharactorData charactor)
    {
        state = State.Atk;
        
        float atkDistanceSeconds = 0.5f;
        rigid.velocity = Vector2.zero;
        animator.SetBool("IsAttack", true);

        while(charactor.Health > 0 && isAlive)
        {
            while (state == State.Stun)//����ѣ��״̬ �����д���
            {
                animator.SetBool("IsAttack", false);
                yield return 1;
            }
            yield return true;

            state = State.Atk;

            charactor.Health -= Data.AtkPower;
            yield return new WaitForSecondsRealtime(atkDistanceSeconds);
        }
        //ֹͣ��������
        animator.SetBool("IsAttack", false);
        state = State.Walk;

        atkCoroutine = null;
    }
    /// <summary>
    /// ��ѣ��
    /// </summary>
    void Stun()
    {
        state = State.Stun;
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
                    return new Color(1f, 0.25f, 0);
                case Elements.Ice:
                    return new Color(0, 1f, 1f);
                case Elements.Electric:
                    return new Color(0.75f, 0, 1f);
                //����Ҳ��ḽ��
                case Elements.Grass:
                    return Color.green;
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
    /// ������ͼɾ��
    /// </summary>
    public void CheckLocation()
    {
        if (GameController.Instance.WorldToGrid(transform.position) == new Vector2(-1, -1))
            Destroy(gameObject);
    }

    public void Update()
    {
        //����ͨħ��Ч������������Data�������Ч�����з���
        handler.CheckEffect(Data.GetEffects());
        if (isAlive)
        {
            //����������û��ѣ��Ч��
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
            //������ɫ
            Color c = CalculateElementColor();
            if (damageCoroutine != null)//���ڻ�����ʾ�˺���Ч��֡��
                c = (c + DamageColor) / 2;
            sprite.color = c;
        }
        if (Data.Health <= 0 && isAlive)//ֻ�л��ŵ�ʱ���������
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
        float secondsBeforeBodyDisappear = 2;
        Color elementColor = CalculateElementColor();//�����ʱ���ŵ���ɫ
        sprite.color = elementColor;
        FallingHead.GetComponent<SpriteRenderer>().color = elementColor ;//�ѵ���ͷ���óɺ��Լ�һ������ɫ
        animator.Play("Die");
        FallingHead.SetActive(true);
        yield return new WaitForSecondsRealtime(secondsBeforeBodyDisappear);

        Data.RemoveOnReceiveAllDamageListener(OnDamage);//�Ƴ�����
        Destroy(gameObject);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        IMonsterAttackable attackable = collision.gameObject.GetComponent<IMonsterAttackable>();
        if (attackable != null && atkCoroutine == null)
        {
            ICharactorData data = attackable.GetData();
            //���������������֡���ã���Data�ĸ�ֵ������Ⱦ֡���ã�
            //�շ���һ��ֲ���ʱ�����׳��� �������Ѿ���������Data��δ��ֵ�����
            if (data != null)
                atkCoroutine = StartCoroutine(AtkCoroutine(data));
        }
    }
    public IDamageReceiver GetReceiver()
    {
        return Data;
    }
}
