using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����
/// </summary>
public class Overloaded : ElementsReaction
{
    public const int OVERLOAD_DAMAGE = 30;
    private static int radius = 20;//Ӱ�췶Χ
    private static int strengthChange = -60;
    private static int changeTime = 1000;
    public override string ReactionName => "Overloaded";

    public override void Action(IElementalDamage damage, IDamageReceiver target)
    {
        target.AddEffect(new StrengthEffect(strengthChange, changeTime, system));
        target.ReceiveDamage(new SystemDamage(OVERLOAD_DAMAGE, Elements.Fire));
    }
}
