using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����,�������һ�ַ�Ӧ��Ҳ��һ��Ч��
/// </summary>
public class Frozen : ElementsReaction, IEffect
{
    /// <summary>
    /// ����˺�����ħ�ﴦ�ڶ���״̬��ͬʱ�ܵ������˺�����ᴥ�����Ч������˺�
    /// </summary>
    public const int BROKE_ICE_DAMAGE = 20;

    /// <summary>
    /// ����ʱ�䣨���룩
    /// </summary>
    private int stunTime = 5000;
    /// <summary>
    /// ����˥��ʱ�䣨���룩
    /// </summary>
    private int speedTime = 8000;
    /// <summary>
    /// ����˥���ٷֱ�
    /// </summary>
    private float speedPercent = -0.5f;

    private IDamageReceiver target;

    private StunEffect stun;
    public StunEffect Stun => stun;
    private SpeedEffect speed;
    public SpeedEffect Speed => speed;

    public string EffectName => Name;

    public override string ReactionName => Name;
    public static string Name => "Frozen";

    public EffectState State
    {
        get
        {
            if (stun.State == EffectState.Initialized && speed.State == EffectState.Initialized)
            {
                return EffectState.Initialized;
            }
            else if (stun.State == EffectState.End && speed.State == EffectState.End)
            {
                return EffectState.End;
            }
            else
                return EffectState.Processing;
        }
    }

    public IGameobjectData Caster => system;

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        stun = new StunEffect(system, stunTime);
        speed = new SpeedEffect(system, speedPercent, speedTime);
        this.target = target;
        target.AddEffect(this);
    }

    void FrozenEffect_OnIceReacted(ElementsReaction reaction)//�ܵ�����Ч��ʱ ����Ԫ�ظ�����ʧ �������߼�
    {
        if (reaction.ReactionName != Frozen.Name) //�����˱��Ԫ�ط�Ӧ
        {
            target.RemoveEffect(this);//�Ƴ�����Ч��
            target.RemoveOnElementReactedListener(Elements.Ice, FrozenEffect_OnIceReacted);
        }
    }
    void FrozenEffect_OnReceivePhysicalDamage(IElementalDamage damage)//���ڶ���������ܵ������˺�ʱ�������Ч��
    {
        target.RemoveEffect(this);
        target.Health -= Frozen.BROKE_ICE_DAMAGE;//ֱ�Ӽ�������ֵ�������ʵ�˺�

        target.RemoveOnReceiveDamageListener(Elements.None, FrozenEffect_OnReceivePhysicalDamage);
    }
    /// <summary>
    /// Ĭ������Ч���߼�
    /// </summary>
    /// <param name="target"></param>
    public void EnableEffect(IGameobjectData target)
    {
        stun.EnableEffect(target);
        speed.EnableEffect(target);
        
        this.target.AddElement(Elements.Ice);//��ӱ�Ԫ�ظ���
        this.target.AddOnElementReactedListener(Elements.Ice, FrozenEffect_OnIceReacted);//���Ԫ�ظ��ż���
        this.target.AddOnReceiveDamageListener(Elements.None, FrozenEffect_OnReceivePhysicalDamage);//��������˺�����
    }
    /// <summary>
    /// Ĭ�Ϲر�Ч���߼�
    /// </summary>
    /// <param name="target"></param>
    public void DisableEffect(IGameobjectData target)
    {
        stun.DisableEffect(target);
        speed.DisableEffect(target);
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
    }
}
