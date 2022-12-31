using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɢ
/// </summary>
public class Swirl : ElementsReaction
{
    private static float radius = 0.8f;//��ɢ��Χ
    private static int SWIRL_DAMAGE = 5;//��ɢ�˺�

    private Elements swirledElement;
    public Swirl(Elements element)
    {
        swirledElement = element;
    }
    public override string ReactionName => "Swirl";


    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                IDamageReceiver receiver = damageable.GetReceiver();
                if(receiver.Equals(target))
                    receiver.ReceiveDamage(new SystemDamage(SWIRL_DAMAGE, swirledElement)); //����Ŀ�겻���ܵ�����
                else
                    receiver.ReceiveDamage(new SystemDamage(SWIRL_DAMAGE, swirledElement, true));
            }
        }
    }
}
