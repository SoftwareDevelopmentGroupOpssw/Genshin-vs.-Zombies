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
    /// <param name="speed">改变的移速数值引用</param>
    public void EnableEffect(ref int speed)
    {
        deltaSpeed = System.Convert.ToInt32(speed * percent);
        speed += deltaSpeed;
        Start();
    }
    /// <summary>
    /// 将改变的移速值返还
    /// </summary>
    /// <param name="speed">改变的移速数值的引用</param>
    public void DisableEffect(ref int speed)
    {
        speed -= deltaSpeed;
    }
}
