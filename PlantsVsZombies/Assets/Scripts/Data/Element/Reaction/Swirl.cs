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

    public override void Action(IElementalDamage damage, IDamageReceiver target)
    {
        ////·¶Î§¼ì²â
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        //foreach (var collider in colliders)
        //{
        //    if (collider.gameObject.tag == "Monster")
        //    {
        //        collider.GetComponent<Monster>().Data.ReceiveDamage(new SystemDamage(swirlDamage, damage.ElementType, true));
        //    }
        //}
    }
}
