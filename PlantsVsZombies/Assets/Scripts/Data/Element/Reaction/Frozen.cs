using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 冻结,冻结既是一种反应，也是一种效果
/// </summary>
public class Frozen : ElementsReaction, IEffect, IContainedStunEffect
{
    /// <summary>
    /// 碎冰伤害：当魔物处于冻结状态下同时受到物理伤害，则会触发碎冰效果造成伤害
    /// </summary>
    public const int BROKE_ICE_DAMAGE = 35;

    /// <summary>
    /// 冻结时间（毫秒）
    /// </summary>
    private int stunTime = 1000;
    /// <summary>
    /// 移速衰减时间（毫秒）
    /// </summary>
    private int speedTime = 3000;
    /// <summary>
    /// 移速衰减百分比
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

    void FrozenEffect_OnIceReacted(ElementsReaction reaction)//受到冻结效果时 若冰元素附着消失 触发的逻辑
    {
        State = EffectState.End;//结束冻结效果
    }
    void FrozenEffect_OnReceivePhysicalDamage(IElementalDamage damage)//当在冻结情况下受到物理伤害时触发碎冰效果
    {
        State = EffectState.End;//结束冻结效果
        target.Health -= Frozen.BROKE_ICE_DAMAGE;//直接减少生命值，造成真实伤害
    }
    /// <summary>
    /// 默认启动效果逻辑
    /// </summary>
    /// <param name="target"></param>
    public void EnableEffect(IGameobjectData target)
    {
        stun.EnableEffect(this.target);
        speed.EnableEffect(this.target);

        this.target.AddElement(Elements.Ice);//添加冰元素附着
        this.target.AddOnElementReactedListener(Elements.Ice, FrozenEffect_OnIceReacted);//添加元素附着监听
        this.target.AddOnReceiveDamageListener(Elements.None, FrozenEffect_OnReceivePhysicalDamage);//添加物理伤害监听

        State = EffectState.Processing;
    }
    /// <summary>
    /// 默认关闭效果逻辑
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
    /// 默认帧更新逻辑
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
