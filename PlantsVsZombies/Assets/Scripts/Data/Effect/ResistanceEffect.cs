using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 改变抗性效果
/// </summary>
public class ResistanceEffect : CountDownEffect
{
    private Elements element;
    private float percent;
    private int duration;
    private IGameobjectData caster;
    /// <summary>
    /// 抗性改变效果
    /// </summary>
    /// <param name="element">抗性的元素类型</param>
    /// <param name="changePercent">抗性改变的百分比，可以为负数</param>
    /// <param name="milisecondsDuration">抗性改变的持续时间</param>
    /// <param name="caster">抗性改变的释放者</param>
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
