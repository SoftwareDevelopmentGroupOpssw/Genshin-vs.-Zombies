using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ѣ��Ч��
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
    /// ����ѣ��Ч��
    /// </summary>
    public override void EnableEffect(IGameobjectData target)
    {
        Start();
    }
    public override void UpdateEffect(IGameobjectData target)
    {
        if(!(target is ICharactorData))
        {
            State = EffectState.Error;
            throw new System.NotSupportedException("Stun effect can only be added to charactors.");
        }
        ICharactorData data = target as ICharactorData;
        data.CanAction = false;
    }
}
