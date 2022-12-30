using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 结晶
/// </summary>
public class Crystallize : ElementsReaction
{
    private const string CRYSTAL_PATH = "";
    private static GameObject crystal;//结晶生成的晶片，触发时生成一个
    public static GameObject Crystal
    {
        get
        {
            if(crystal == null)
            {
                //crystal = ResourceManager.Instance.Load<GameObject>(CRYSTAL_PATH);//去加载晶片物体
            }
            return crystal;
        }
    }

    private int crystallizeDamage = 10;

    public override string ReactionName => "Crystallize";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        damage.AtkDmg += crystallizeDamage;

        //在怪物的脚下生成一个结晶晶片
        GameObject obj = GameObject.Instantiate(Crystal);
        obj.transform.position = target.GameObject.transform.position;
    }
}
