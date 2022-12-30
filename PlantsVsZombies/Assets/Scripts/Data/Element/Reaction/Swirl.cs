using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// À©É¢
/// </summary>
public class Swirl : ElementsReaction
{
    private int radius = 20;//À©É¢·¶Î§
    private int swirlDamage = 5;//À©É¢ÉËº¦
    public override string ReactionName => "Swirl";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        //·¶Î§¼ì²â
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.GetReceiver().ReceiveDamage(new SystemDamage(swirlDamage, damage.ElementType, true));
            }
        }
    }
}
