using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frozen : ElementsReaction
{
    /// <summary>
    /// ����ʱ�䣨���룩
    /// </summary>
    private int stunTime = 500;
    /// <summary>
    /// ����˥��ʱ�䣨���룩
    /// </summary>
    private int speedTime = 2000;
    /// <summary>
    /// ����˥���ٷֱ�
    /// </summary>
    private float speedPercent = -0.5f;


    public override void Action(IElementalDamage damage, IMonsterData target)
    {
        target.AddEffect(new StunEffect(system, stunTime));
        target.AddEffect(new SpeedEffect(system, speedPercent, speedTime));
    }
}
