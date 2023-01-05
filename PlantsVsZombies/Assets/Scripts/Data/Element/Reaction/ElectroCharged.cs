using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �е磬����һ�ַ�Ӧ����һ��Ч��
/// </summary>
public class ElectroCharged : ElementsReaction, IEffect
{
    private static float radius = 1.0f;   //�е��˺��İ뾶
    private static int damageTimes = 4;    //�е�����˺�����
    private static int damageDealtPerTime = 10;//һ�θе�����˺�ֵ
    private static int damageSpaceTime = 500;//���θе��˺�֮��ļ��ʱ�䣨���룩
    private static int deltaStrength = -30;//���ڹ�������ɵ�����˥��ֵ
    private StrengthEffect strength;
    /// <summary>
    /// ����е�Ч���ܷ����Ԫ��
    /// </summary>
    private bool canAddElement = true;

    private IDamageReceiver target;//�����е練Ӧ��Ŀ��
    public StrengthEffect Strength => strength;
    public ElectroCharged()
    {
        strength = new StrengthEffect(deltaStrength, damageTimes * damageSpaceTime, system);
        State = EffectState.Initialized;
    }

    public override string ReactionName => Name;
    public static string Name => "Electro-Charged";

    public string EffectName => Name;

    public EffectState State { get; private set; }
    public IGameobjectData Caster => system;

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        //��Χ���:��Χ�ڵ�ʩ�Ӹе�Ч��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();//��ȡ���Դ����е�Ķ���
            //�����ܵ��˺�
            if (damageable != null && damageable is Monster)
            {
                //��ȡһ���˺�������
                IDamageReceiver receiver = damageable.GetReceiver();
                List<IEffect> effects = receiver.GetEffects();
                bool hasCanAddElement = false; // ��û���ܼ�Ԫ�ط�Ӧ�ĸе�
                bool hasCannotAddElement = false; //��û�в��ܼ�Ԫ�صĸе�
                if (effects != null)
                {
                    effects.ForEach((effect) =>
                    {
                        if (effect is ElectroCharged)
                        {
                            ElectroCharged electroCharged = effect as ElectroCharged;
                            electroCharged.RefreshStatus();//ˢ�����ϵĸе�Ч��
                            if (electroCharged.canAddElement)
                                hasCanAddElement = true;
                            else
                                hasCannotAddElement = true;
                        }
                    });
                }
                if (receiver.Equals(target) && !hasCanAddElement)
                {
                    this.target = receiver;
                    receiver.AddEffect(this);
                }
                else if(!receiver.Equals(target) && !hasCannotAddElement)
                {
                    receiver.AddEffect(new ElectroCharged() { canAddElement = false , target = receiver });
                }
            }
        }
    }
    /// <summary>
    /// ��ɳ����˺���Э��
    /// </summary>
    private Coroutine damageCoroutine;
    private int nowTimes = 0;
    private IEnumerator DamageCoroutine(IDamageReceiver target)
    {
        for (;nowTimes < damageTimes; nowTimes++)
        {
            target.ReceiveDamage(new SystemDamage(damageDealtPerTime, Elements.Electric));
            yield return new WaitForSeconds(damageSpaceTime / 1000f);
        }
        State = EffectState.End;
    }

    private void RefreshStatus()
    {
        nowTimes = 0;
        if (canAddElement)
        {
            this.target.AddElement(Elements.Electric);
            this.target.AddElement(Elements.Water);
        }
    }

    /// <summary>
    /// ��ˮ��Ԫ�ر���Ӧ��ʱ����Ϊ�е練Ӧ��ˢ�£����򶼽�ԭ���ĸе練Ӧ��ˮ��Ԫ���Ƴ�
    /// </summary>
    /// <param name="reaction"></param>
    void ElectroCharged_OnElementReacted(ElementsReaction reaction)
    {
        if(!(reaction is ElectroCharged))
        {
            State = EffectState.End;//�е�Ч������
        }
    }
    public void EnableEffect(IGameobjectData target)
    { 
        //���������˺���Э��
        damageCoroutine = MonoManager.Instance.StartCoroutine(DamageCoroutine(this.target));
        State = EffectState.Processing;
        
        if (canAddElement)
        {
            //���ˮ�׹���
            this.target.AddElement(Elements.Electric);
            this.target.AddElement(Elements.Water);
            //���Ԫ�ط�Ӧ����
            this.target.AddOnElementReactedListener(Elements.Water, ElectroCharged_OnElementReacted);
            this.target.AddOnElementReactedListener(Elements.Electric, ElectroCharged_OnElementReacted);
        }
        
        strength.EnableEffect(this.target);//������������Ч��
    }

    public void DisableEffect(IGameobjectData target)
    {

        strength.DisableEffect(this.target);

        if (canAddElement)
        {
            //���Ƴ�ʱ�Ƴ�ˮ��Ԫ��
            this.target.RemoveElement(Elements.Electric);
            this.target.RemoveElement(Elements.Water);
            //�Ƴ�����
            this.target.RemoveOnElementReactedListener(Elements.Water, ElectroCharged_OnElementReacted);
            this.target.RemoveOnElementReactedListener(Elements.Electric, ElectroCharged_OnElementReacted);
        }
        if(damageCoroutine != null)
            MonoManager.Instance.StopCoroutine(damageCoroutine);//���Ƴ�ʱֹͣ�˺�Э��
    }

    public void UpdateEffect(IGameobjectData target)
    {
        
    }
}
