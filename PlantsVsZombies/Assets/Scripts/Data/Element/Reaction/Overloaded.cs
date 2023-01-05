using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 超载
/// </summary>
public class Overloaded : ElementsReaction
{
    public const int OVERLOAD_DAMAGE = 50;
    private static float radius = 0.6f;//影响范围
    private static float sputterDamagePercent = 0.65f;//范围伤害比主要伤害的百分比
    private static int strengthChange = -60;
    private static int changeTime = 1000;
    public override string ReactionName => "Overloaded";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        target.AddEffect(new StrengthEffect(strengthChange, changeTime, system));//只给主要目标加削韧效果
        
        //范围检测
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null && damageable is Monster)
            {
                IDamageReceiver receiver = damageable.GetReceiver();
                if(receiver.Equals(target))
                    receiver.ReceiveDamage(new SystemDamage(OVERLOAD_DAMAGE, Elements.Fire));
                else

                    receiver.ReceiveDamage(new SystemDamage((int)(OVERLOAD_DAMAGE * sputterDamagePercent), Elements.Fire));
            }
        }
    }
}
