using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �е磬����һ�ַ�Ӧ����һ��Ч��
/// </summary>
public class ElectroCharged : ElementsReaction, IEffect
{
    private static float radius = 1.2f;   //�е��˺��İ뾶
    private static int damageTimes = 4;    //�е�����˺�����
    private static int damageDealtPerTime = 5;//һ�θе�����˺�ֵ
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
        this.target = target;
        target.AddEffect(this);//Ϊ��ҪĿ���һ�������Ԫ�صĸе�Ч��

        //��Χ���:��Χ�ڵ�ʩ�Ӹе�Ч��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();//��ȡ���Դ����е�Ķ���
            //�����ܵ��˺�
            if (damageable != null)
            {
                //��ȡһ���˺�������
                IDamageReceiver receiver = damageable.GetReceiver();
                List<IEffect> effects = receiver.GetEffects();
                if(effects != null)
                {
                    bool contained = !effects.TrueForAll((effect) => !(effect is ElectroCharged));
                    if (!contained)
                        receiver.AddEffect(new ElectroCharged() { target = receiver, canAddElement = false });//�Է�Χ�ڵĵ�������˺�����������Ԫ��
                }
                else
                {
                    receiver.AddEffect(new ElectroCharged() { target = receiver, canAddElement = false });//�Է�Χ�ڵĵ�������˺�����������Ԫ
                }
            }
        }
    }
    /// <summary>
    /// ��ɳ����˺���Э��
    /// </summary>
    private Coroutine damageCoroutine;
    private IEnumerator DamageCoroutine(IDamageReceiver target)
    {
        for (int i = 0; i< damageTimes; i++)
        {
            target.ReceiveDamage(new SystemDamage(damageDealtPerTime, Elements.Electric));
            yield return new WaitForSecondsRealtime((float)damageSpaceTime / 1000);
        }
        State = EffectState.End;
    }
    /// <summary>
    /// ��ˮ��Ԫ�ر���Ӧ��ʱ�������·�Ӧ�Ǹе绹��������Ӧ������ԭ���ĸе練Ӧ��ˮ��Ԫ���Ƴ�
    /// </summary>
    /// <param name="reaction"></param>
    void ElectroCharged_OnElementReacted(ElementsReaction reaction)
    {
        State = EffectState.End;//�е�Ч������
        target.RemoveOnElementReactedListener(Elements.Water, ElectroCharged_OnElementReacted);
        target.RemoveOnElementReactedListener(Elements.Electric, ElectroCharged_OnElementReacted);
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
        MonoManager.Instance.StopCoroutine(damageCoroutine);//���Ƴ�ʱֹͣ�˺�Э��
    }

    public void UpdateEffect(IGameobjectData target)
    {
        
    }
}
