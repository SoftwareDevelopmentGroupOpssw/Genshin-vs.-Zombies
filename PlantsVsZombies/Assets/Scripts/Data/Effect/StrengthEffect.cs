using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ըı�Ч��
/// </summary>
public class StrengthEffect : CountDownEffect
{
    private int duration;
    private int changeValue;
    public IGameobjectData caster;
    /// <summary>
    /// ����һ�����Ըı�Ч��
    /// </summary>
    /// <param name="changeValue">�ı��ֵ������Ϊ����</param>
    /// <param name="milisecondsDuration">����ʱ��</param>
    /// <param name="caster">ʩ����</param>
    public StrengthEffect(int changeValue, int milisecondsDuration, IGameobjectData caster)
    {
        this.changeValue = changeValue;
        duration = milisecondsDuration;
        this.caster = caster;
    }

    public override int MilisecondsDuration => duration;

    public override string EffectName => "StrengthEffect";

    public override IGameobjectData Caster { get => caster; }

    public override void EnableEffect(IGameobjectData target)
    {
        if(!(target is IMonsterData))
        {
            State = EffectState.Error;
            throw new System.NotSupportedException("Strength effect can only be added on monsters");
        }
        IMonsterData monster = target as IMonsterData;
        monster.Strength += changeValue;
        Start();
    }
    public override void DisableEffect(IGameobjectData target)
    {
        IMonsterData monster = target as IMonsterData;
        monster.Strength -= changeValue;
    }
}
