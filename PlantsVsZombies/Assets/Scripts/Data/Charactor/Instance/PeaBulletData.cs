using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ͨ�÷��������ݣ��㶹
/// </summary>
public class PeaBulletData : IFlyerData, IElementalDamage
{
    private Elements element;
    private int damage;
    public PeaBulletData(GameObject original,Elements element, int damage)
    {
        OriginalReference = original;
        this.element = element;
        this.damage = damage;
    }

    public GameObject GameObject { get; set; }

    public GameObject OriginalReference { get; private set; }

    public int AtkDmg { get => damage; set => damage = value; }
    public Elements ElementType { get => element; set => element = value; }

    public bool CanAddElement { get; set; } = true;//�㶹�ӵ�һֱ�������Ԫ��

    public void OnTriggered(GameObject target)
    {
        IGameobjectData data = target.GetComponent<BaseGameobject>().Data;
        if(data is IDamageReceiver)
        {
            (data as IDamageReceiver).ReceiveDamage(this);
        }
    }


    public void AddEffect(IEffect effect)
    {
        
    }

    public List<IEffect> GetEffects()
    {
        return null;
    }

    public void RemoveEffect(IEffect effect)
    {
        
    }
}
