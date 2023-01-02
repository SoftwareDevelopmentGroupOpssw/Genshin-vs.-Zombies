using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����,�������һ�ַ�Ӧ��Ҳ��һ��Ч��
/// </summary>
public class Frozen : ElementsReaction, IEffect, IContainedStunEffect
{
    /// <summary>
    /// ����˺�����ħ�ﴦ�ڶ���״̬��ͬʱ�ܵ������˺�����ᴥ�����Ч������˺�
    /// </summary>
    public const int BROKE_ICE_DAMAGE = 35;

    /// <summary>
    /// ����ʱ�䣨���룩
    /// </summary>
    private int stunTime = 1000;
    /// <summary>
    /// ����˥��ʱ�䣨���룩
    /// </summary>
    private int speedTime = 3000;
    /// <summary>
    /// ����˥���ٷֱ�
    /// </summary>
    private float speedPercent = -0.3f;

    private IDamageReceiver target;

    private StunEffect stun;
    public StunEffect Stun => stun;
    
    private SpeedEffect speed;
    public SpeedEffect Speed => speed;

    public string EffectName => Name;

    public override string ReactionName => Name;
    public static string Name => "Frozen";

    public EffectState State { get; private set; }

    public IGameobjectData Caster => system;

    public bool IsStunEffectOver => stun.IsStunEffectOver;

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        stun = new StunEffect(system, stunTime);
        speed = new SpeedEffect(system, speedPercent, speedTime);
        this.target = target;
        target.AddEffect(this);
        State = EffectState.Initialized;
    }

    void FrozenEffect_OnIceReacted(ElementsReaction reaction)//�ܵ�����Ч��ʱ ����Ԫ�ظ�����ʧ �������߼�
    {
        State = EffectState.End;//��������Ч��
    }
    void FrozenEffect_OnReceivePhysicalDamage(IElementalDamage damage)//���ڶ���������ܵ������˺�ʱ�������Ч��
    {
        State = EffectState.End;//��������Ч��
        target.Health -= Frozen.BROKE_ICE_DAMAGE;//ֱ�Ӽ�������ֵ�������ʵ�˺�
    }
    /// <summary>
    /// Ĭ������Ч���߼�
    /// </summary>
    /// <param name="target"></param>
    public void EnableEffect(IGameobjectData target)
    {
        stun.EnableEffect(this.target);
        speed.EnableEffect(this.target);

        this.target.AddElement(Elements.Ice);//��ӱ�Ԫ�ظ���
        this.target.AddOnElementReactedListener(Elements.Ice, FrozenEffect_OnIceReacted);//���Ԫ�ظ��ż���
        this.target.AddOnReceiveDamageListener(Elements.None, FrozenEffect_OnReceivePhysicalDamage);//��������˺�����

        State = EffectState.Processing;
    }
    /// <summary>
    /// Ĭ�Ϲر�Ч���߼�
    /// </summary>
    /// <param name="target"></param>
    public void DisableEffect(IGameobjectData target)
    {
        stun.DisableEffect(this.target);
        speed.DisableEffect(this.target);

        this.target.RemoveOnReceiveDamageListener(Elements.None, FrozenEffect_OnReceivePhysicalDamage);
        this.target.RemoveOnElementReactedListener(Elements.Ice, FrozenEffect_OnIceReacted);
    }
    /// <summary>
    /// Ĭ��֡�����߼�
    /// </summary>
    /// <param name="target"></param>
    public void UpdateEffect(IGameobjectData target)
    {
        if(stun.State == EffectState.Processing)
            stun.UpdateEffect(target);
        if(speed.State == EffectState.Processing)
            speed.UpdateEffect(target);
        if (stun.State == EffectState.End && speed.State == EffectState.End)
            State = EffectState.End;
    }
}
