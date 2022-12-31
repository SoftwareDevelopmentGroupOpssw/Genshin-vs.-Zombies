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

}
