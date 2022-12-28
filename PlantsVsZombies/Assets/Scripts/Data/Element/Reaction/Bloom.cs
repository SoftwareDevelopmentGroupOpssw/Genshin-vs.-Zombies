using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 绽放
/// </summary>
public class Bloom : ElementsReaction
{
    private const string SEED_PATH = "";
    private static GameObject seed; //绽放生成的种子
    public static GameObject Seed
    {
        get
        {
            if (seed == null)
            {
                //seed = ResourceManager.Instance.Load<GameObject>(SEED_PATH);//去加载种子物体
            }
            return seed;
        }
    }
    public override string ReactionName => "Bloom";

    public override void Action(IElementalDamage damage, IDamageReceiver target)
    {
        //在怪物的脚下生成一个草种子
        GameObject obj = GameObject.Instantiate(Seed);
        obj.transform.position = target.GameObject.transform.position;
    }
}
