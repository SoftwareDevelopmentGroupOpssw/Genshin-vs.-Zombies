using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 冻结
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

    private StunEffect stun;
    public StunEffect Stun => stun;
    private SpeedEffect speed;
    public SpeedEffect Speed => speed;

    public string EffectName => "Frozen";

    public override string Name => "Frozen";

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

    public override void Action(IElementalDamage damage, IMonsterData target)
    {
        stun = new StunEffect(system, stunTime);
        speed = new SpeedEffect(system, speedPercent, speedTime);
        target.AddEffect(this);

    }
}
