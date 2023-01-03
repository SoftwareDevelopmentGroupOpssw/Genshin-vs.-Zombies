using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ч���к���ѣ��Ч���Ľӿ�
/// </summary>
public interface IContainedStunEffect : IEffect
{
    /// <summary>
    /// ��Ч���еĲ���ѣ��Ч���Ƿ����
    /// </summary>
    /// <returns></returns>
    public bool IsStunEffectOver { get; }
}

/// <summary>
/// ѣ��Ч��
/// </summary>
public class StunEffect : CountDownEffect, IContainedStunEffect
{
    private IGameobjectData caster;
    private int duration;
    /// <summary>
    /// ����һ��ѣ��Ч��
    /// </summary>
    /// <param name="caster">ʩ����</param>
    /// <param name="milisecondsDuration">ѣ��Ч������ʱ��</param>
    public StunEffect(IGameobjectData caster,int milisecondsDuration)
    {
        this.caster = caster;
        duration = milisecondsDuration;
    }

    public override int MilisecondsDuration => duration;

    public override string EffectName => "Stun";

    public override IGameobjectData Caster => caster;

    public bool IsStunEffectOver { get => CountDown.Available; }

    public override void EnableEffect(IGameobjectData target)
    {
        Start();
    }
    public override void DisableEffect(IGameobjectData target)
    {
        try
        {
            CountDown.Reset();
        }
        catch
        {
            Debug.Log(CountDown is null);
        }
    }
}
