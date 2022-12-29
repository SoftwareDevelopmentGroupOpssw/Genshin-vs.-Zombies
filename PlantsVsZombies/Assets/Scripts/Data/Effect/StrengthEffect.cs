using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 韧性改变效果
/// </summary>
public class StrengthEffect : CountDownEffect
{
    private int duration;
    private int changeValue;
    public IGameobjectData caster;
    /// <summary>
    /// 创建一个韧性改变效果
    /// </summary>
    /// <param name="changeValue">改变的值，可以为负数</param>
    /// <param name="milisecondsDuration">持续时间</param>
    /// <param name="caster">施加者</param>
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
