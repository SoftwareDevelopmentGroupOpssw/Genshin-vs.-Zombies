using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ı俹��Ч��
/// </summary>
public class ResistanceEffect : CountDownEffect
{
    private Elements element;
    private float percent;
    private int duration;
    private IGameobjectData caster;
    /// <summary>
    /// ���Ըı�Ч��
    /// </summary>
    /// <param name="element">���Ե�Ԫ������</param>
    /// <param name="changePercent">���Ըı�İٷֱȣ�����Ϊ����</param>
    /// <param name="milisecondsDuration">���Ըı�ĳ���ʱ��</param>
    /// <param name="caster">���Ըı���ͷ���</param>
    public ResistanceEffect(Elements element,float changePercent ,int milisecondsDuration,IGameobjectData caster)
    {
        this.element = element;
        duration = milisecondsDuration;
        percent = changePercent;
        this.caster = caster;
    }
    public override void EnableEffect(IGameobjectData target)
    {
        if(!(target is IMonsterData))
        {
            State = EffectState.Error;
            throw new System.NotSupportedException("Resistance effect can only be added on monsters");
        }
        IMonsterData monster = target as IMonsterData;
        monster.SetResistance(monster.GetResistance(element) + percent, element);
        Start();
    }
    public override void DisableEffect(IGameobjectData target)
    {
        IMonsterData monster = target as IMonsterData;
        monster.SetResistance(monster.GetResistance(element) - percent, element);
    }

    public override int MilisecondsDuration => duration;
    
    public override string EffectName => "ResistanceEffect";

    public override IGameobjectData Caster => caster;
}
