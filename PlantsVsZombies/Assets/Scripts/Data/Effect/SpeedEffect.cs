using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 移速加成效果
/// </summary>
public class SpeedEffect : CountDownEffect
{
    /// <summary>
    /// 效果造成的移速改变值
    /// </summary>
    private int deltaSpeed;

    private float percent;
    /// <summary>
    /// 移动效果的百分比
    /// </summary>
    public float Percent => percent;

    public override string EffectName => "SpeedEffect";

    private int duration;
    public override int MilisecondsDuration { get => duration; }

    private IGameobjectData caster;
    public override IGameobjectData Caster => caster;

    /// <summary>
    /// 生成移速效果
    /// </summary>
    /// <param name="caster">施放者</param>
    /// <param name="percent">移速效果修正百分比，可以为正值也可以为负值</param>
    /// <param name="miliSecondsDuration">持续时间（毫秒）</param>
    public SpeedEffect(IGameobjectData caster, float percent,int miliSecondsDuration)
    {
        this.percent = percent;
        this.caster = caster;
        duration = miliSecondsDuration;
    }
    /// <summary>
    /// 增加/减少移速
    /// </summary>
    /// <param name="target">改变移速的对象</param>
    /// <exception cref="System.NotSupportedException">尝试向一个非怪物单位添加移速效果</exception>
    public override void EnableEffect(IGameobjectData target)
    {
        if (!(target is IMonsterData))//对象不是一个怪物
        {
            Error();
            throw new System.NotSupportedException("The speed effect can only be added on monsters.");//报错：只有怪物身上能添加移速效果
        }
        else
        {
            IMonsterData monster = target as IMonsterData;
            deltaSpeed = System.Convert.ToInt32(monster.Speed * percent);
            monster.Speed += deltaSpeed;
            Start();
        }
    }
    /// <summary>
    /// 将改变的移速值返还
    /// </summary>
    /// <param name="target">改变移速的对象</param>
    public override void DisableEffect(IGameobjectData target)
    {
        //能被添加,说明一定是怪物
        IMonsterData monster = target as IMonsterData;
        monster.Speed -= deltaSpeed;
    }
}
