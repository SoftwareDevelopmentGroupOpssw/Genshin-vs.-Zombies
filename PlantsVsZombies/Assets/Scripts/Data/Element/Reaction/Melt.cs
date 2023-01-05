using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melt : ElementsReaction
{
    public const float DAMAGE_INCREASE = 2.15f;
    public override string ReactionName => Name;
    public static string Name => "Melt";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        damage.Damage = (int)(DAMAGE_INCREASE * damage.Damage);
    }
}
