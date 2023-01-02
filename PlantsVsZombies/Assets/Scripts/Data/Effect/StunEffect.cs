using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 效果中含有眩晕效果的接口
/// </summary>
public interface IContainedStunEffect : IEffect
{
    /// <summary>
    /// 此效果中的部分眩晕效果是否结束
    /// </summary>
    /// <returns></returns>
    public bool IsStunEffectOver { get; }
}

/// <summary>
/// 眩晕效果
/// </summary>
public class StunEffect : CountDownEffect, IContainedStunEffect
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

    public bool IsStunEffectOver { get => countDown.Available; }

    public override void EnableEffect(IGameobjectData target)
    {
        Start();
    }
    public override void DisableEffect(IGameobjectData target)
    {
        countDown.Reset();
    }
}
