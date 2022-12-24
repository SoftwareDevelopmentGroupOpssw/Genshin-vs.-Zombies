using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterData : IMonsterData
{
    private List<Elements> elements = new List<Elements>(7);

    public abstract int Strength { get; set; }
    public abstract int Health { get; set; }
    public abstract int AtkPower { get; set; }
    public GameObject GameObject { get; set; }

    public abstract string ResourcePath { get; }

    public abstract float GetResistance(Elements element);

    public abstract IGameobjectData Instantiate();

    public abstract void OnAwake();

    public abstract void OnDestroy();

    public void ReceiveDamage(IElementalDamage damage)
    {
        //本地函数：造成伤害
        void PlainDealsDamage(IElementalDamage damage)
        {
            float value = GetResistance(damage.ElementType);
            Health -= (int)(damage.AtkDmg * value);
            elements.Add(damage.ElementType);
        }
        if (damage.CanAddElement)
        {
            Elements causerElement = damage.ElementType;
            int i = 0;
            for (; i < elements.Count; i++)
            {
                Elements element = elements[i];
                ElementsReaction reaction = ElementsReaction.GetReaction(element, causerElement);
                if (reaction != null)
                {
                    reaction.Action(damage, this);
                    elements.RemoveAt(i);
                    break;
                }
            }
            if (i == elements.Count)//没有找到能够反应的元素
                PlainDealsDamage(damage);
        }
        else
            PlainDealsDamage(damage);
        
    }
}
