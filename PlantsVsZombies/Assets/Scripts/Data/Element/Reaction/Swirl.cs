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

    public override void Action(IElementalDamage damage, IDamageReceiver target)
    {
        ////��Χ���
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
