using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frozen : ElementsReaction
{
    /// <summary>
    /// 冻结时间（毫秒）
    /// </summary>
    private int stunTime = 500;
    /// <summary>
    /// 移速衰减时间（毫秒）
    /// </summary>
    private int speedTime = 2000;
    /// <summary>
    /// 移速衰减百分比
    /// </summary>
    private float speedPercent = -0.5f;


    public override void Action(IElementalDamage damage, IMonsterData target)
    {
        target.AddEffect(new StunEffect(system, stunTime));
        target.AddEffect(new SpeedEffect(system, speedPercent, speedTime));
    }
}
