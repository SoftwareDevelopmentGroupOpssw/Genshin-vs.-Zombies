using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
public class Bloom : ElementsReaction
{
    private const string SEED_PATH = "";
    private static GameObject seed; //�������ɵ�����
    public static GameObject Seed
    {
        get
        {
            if (seed == null)
            {
                //seed = ResourceManager.Instance.Load<GameObject>(SEED_PATH);//ȥ������������
            }
            return seed;
        }
    }
    public override string ReactionName => "Bloom";

    public override void Action(IElementalDamage damage, IDamageReceiver target)
    {
        //�ڹ���Ľ�������һ��������
        GameObject obj = GameObject.Instantiate(Seed);
        obj.transform.position = target.GameObject.transform.position;
    }
}
