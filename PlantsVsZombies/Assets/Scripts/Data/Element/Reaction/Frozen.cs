using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 冻结,冻结既是一种反应，也是一种效果
/// </summary>
public class Frozen : ElementsReaction, IEffect
{
    /// <summary>
    /// 碎冰伤害：当魔物处于冻结状态下同时受到物理伤害，则会触发碎冰效果造成伤害
    /// </summary>
    public const int BROKE_ICE_DAMAGE = 20;

    /// <summary>
    /// 冻结时间（毫秒）
    /// </summary>
    private int stunTime = 5000;
    /// <summary>
    /// 移速衰减时间（毫秒）
    /// </summary>
    private int speedTime = 8000;
    /// <summary>
    /// 移速衰减百分比
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

    void FrozenEffect_OnIceReacted(ElementsReaction reaction)//受到冻结效果时 若冰元素附着消失 触发的逻辑
    {
        if (reaction.ReactionName != Frozen.Name) //触发了别的元素反应
        {
            target.RemoveEffect(this);//移除冻结效果
            target.RemoveOnElementReactedListener(Elements.Ice, FrozenEffect_OnIceReacted);
        }
    }
    void FrozenEffect_OnReceivePhysicalDamage(IElementalDamage damage)//当在冻结情况下受到物理伤害时触发碎冰效果
    {
        target.RemoveEffect(this);
        target.Health -= Frozen.BROKE_ICE_DAMAGE;//直接减少生命值，造成真实伤害

        target.RemoveOnReceiveDamageListener(Elements.None, FrozenEffect_OnReceivePhysicalDamage);
    }
    /// <summary>
    /// 默认启动效果逻辑
    /// </summary>
    /// <param name="target"></param>
    public void EnableEffect(IGameobjectData target)
    {
        stun.EnableEffect(target);
        speed.EnableEffect(target);
        
        this.target.AddElement(Elements.Ice);//添加冰元素附着
        this.target.AddOnElementReactedListener(Elements.Ice, FrozenEffect_OnIceReacted);//添加元素附着监听
        this.target.AddOnReceiveDamageListener(Elements.None, FrozenEffect_OnReceivePhysicalDamage);//添加物理伤害监听
    }
    /// <summary>
    /// 默认关闭效果逻辑
    /// </summary>
    /// <param name="target"></param>
    public void DisableEffect(IGameobjectData target)
    {
        stun.DisableEffect(target);
        speed.DisableEffect(target);
    }
    /// <summary>
    /// 默认帧更新逻辑
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
