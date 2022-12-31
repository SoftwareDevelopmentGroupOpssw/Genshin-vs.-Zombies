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
    /// <summary>
    /// 创建一个眩晕效果
    /// </summary>
    /// <param name="caster">施加者</param>
    /// <param name="milisecondsDuration">眩晕效果持续时间</param>
    public StunEffect(IGameobjectData caster,int milisecondsDuration)
    {
        this.caster = caster;
        duration = milisecondsDuration;
    }

    public override int MilisecondsDuration => duration;

    public override string EffectName => "Stun";

    public override IGameobjectData Caster => caster;

}
