using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����
/// </summary>
public class Overloaded : ElementsReaction
{
    public const int OVERLOAD_DAMAGE = 30;
    private static float radius = 0.6f;//Ӱ�췶Χ
    private static int strengthChange = -60;
    private static int changeTime = 1500;
    public override string ReactionName => "Overloaded";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        target.AddEffect(new StrengthEffect(strengthChange, changeTime, system));//ֻ����ҪĿ�������Ч��
        
        //��Χ���
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null && damageable is Monster)
            {
                
                IDamageReceiver receiver = damageable.GetReceiver();
                receiver.ReceiveDamage(new SystemDamage(OVERLOAD_DAMAGE, Elements.Fire));
            }
        }
    }
}
