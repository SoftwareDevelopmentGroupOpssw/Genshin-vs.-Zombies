using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 超导
/// </summary>
public class SuperConduct : ElementsReaction
{
    private static int radius = 10;//超导效果触发的半径
    private static int damage = 10;//超导效果的伤害
    
    public override string ReactionName => "SuperConduct";
    private static float physicalChange = -0.5f;

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        //范围检测
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                IDamageReceiver receiver = damageable.GetReceiver();
                receiver.ReceiveDamage(new SystemDamage(SuperConduct.damage, Elements.Ice));
                receiver.AddEffect(new ResistanceEffect(Elements.None, physicalChange, 6000, system));
            }
        }
    }
}
