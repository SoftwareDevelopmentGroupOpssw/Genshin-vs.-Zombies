using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 眩晕效果
/// </summary>
public class StunEffect : CountDownEffect
{
    private IGameobjectData caster;
    private int duration;
    public StunEffect(IGameobjectData caster,int milisecondsDuration)
    {
        this.caster = caster;
        duration = milisecondsDuration;
    }

    public override int MilisecondsDuration => duration;

    public override string EffectName => "Stun";

    public override IGameobjectData Caster => caster;

    /// <summary>
    /// 启动眩晕效果
    /// </summary>
    public void EnableEffect()
    {
        Start();
    }
}
