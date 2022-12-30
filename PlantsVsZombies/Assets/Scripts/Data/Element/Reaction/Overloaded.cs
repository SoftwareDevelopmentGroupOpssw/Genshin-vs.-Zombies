using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ≥¨‘ÿ
/// </summary>
public class Overloaded : ElementsReaction
{
    public const int OVERLOAD_DAMAGE = 30;
    private static float radius = 0.9f;//”∞œÏ∑∂Œß
    private static int strengthChange = -60;
    private static int changeTime = 1000;
    public override string ReactionName => "Overloaded";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        target.AddEffect(new StrengthEffect(strengthChange, changeTime, system));
        target.ReceiveDamage(new SystemDamage(OVERLOAD_DAMAGE, Elements.Fire));

        //∑∂ŒßºÏ≤‚
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.tag == "Monster")
            {
                Monster monster = collider.gameObject.GetComponent<Monster>();
                if (monster is IDamageable)
                {
                    IDamageReceiver receiver = (monster as IDamageable).GetReceiver();
                    receiver.AddEffect(new StrengthEffect(strengthChange, changeTime, system));
                    receiver.ReceiveDamage(new SystemDamage(OVERLOAD_DAMAGE, Elements.Fire));
                }
            }
        }
    }
}
