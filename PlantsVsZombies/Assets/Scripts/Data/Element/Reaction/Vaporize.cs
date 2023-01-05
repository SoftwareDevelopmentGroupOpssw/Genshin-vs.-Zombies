using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ’Ù∑¢
/// </summary>
public class Vaporize : ElementsReaction
{
    public const float DAMAGE_INCREASE = 2.15f;
    public override string ReactionName => Name;
    public static string Name => "Vaporize";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        damage.Damage = (int) (DAMAGE_INCREASE * damage.Damage);
    }
}
