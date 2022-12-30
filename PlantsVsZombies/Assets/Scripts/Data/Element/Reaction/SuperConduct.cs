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
        target.ReceiveDamage(new SystemDamage(SuperConduct.damage, Elements.Ice));
        target.AddEffect(new ResistanceEffect(Elements.None, physicalChange, 6000, system));

        ////范围检测
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        //foreach (var collider in colliders)
        //{
        //    if (collider.gameObject.tag == "Monster")
        //    {
        //        IMonsterData monster = collider.GetComponent<Monster>().Data;
        //        monster.ReceiveDamage(new SystemDamage(SuperConduct.damage, Elements.Ice));
        //        monster.AddEffect(new ResistanceEffect(Elements.None, physicalChange, 6000, system));
        //    }
        //}
    }
}
