using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ᾧ
/// </summary>
public class Crystallize : ElementsReaction
{
    private const string CRYSTAL_PATH = "";
    private static GameObject crystal;//�ᾧ���ɵľ�Ƭ������ʱ����һ��
    public static GameObject Crystal
    {
        get
        {
            if(crystal == null)
            {
                //crystal = ResourceManager.Instance.Load<GameObject>(CRYSTAL_PATH);//ȥ���ؾ�Ƭ����
            }
            return crystal;
        }
    }

    private int crystallizeDamage = 10;

    public override string ReactionName => "Crystallize";

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        damage.AtkDmg += crystallizeDamage;

        //�ڹ���Ľ�������һ���ᾧ��Ƭ
        GameObject obj = GameObject.Instantiate(Crystal);
        obj.transform.position = target.GameObject.transform.position;
    }
}
