using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɢ
/// </summary>
public class Swirl : ElementsReaction
{
    private int radius = 20;//��ɢ��Χ
    private int swirlDamage = 5;//��ɢ�˺�
    public override string ReactionName => "Swirl";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        //��Χ���
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
