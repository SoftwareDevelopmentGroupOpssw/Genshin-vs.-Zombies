using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melt : ElementsReaction
{
    public const float DAMAGE_INCREASE = 1.75f;
    public override string Name => "Melt";

    public override void Action(IElementalDamage damage, IMonsterData target)
    {
        damage.AtkDmg = (int)(DAMAGE_INCREASE * damage.AtkDmg);
    }
}
