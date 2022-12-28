using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MonsterData : IMonsterData, IDamageReceiver
{
    /// <summary>
    /// 身上的效果
    /// </summary>
    private List <IEffect> effects = new List<IEffect>();

    /// <summary>
    /// 附着的元素
    /// </summary>
    private ElementalObject<bool> elements = new ElementalObject<bool>();

    /// <summary>
    /// 抗性值
    /// </summary>
    private ElementalObject<float> resistances = new ElementalObject<float>();

    /// <summary>
    /// 当受到此元素类型伤害时调用的函数
    /// </summary>
    private ElementEvent<IElementalDamage> OnReceivedDamage = new ElementEvent<IElementalDamage>();
    public void AddOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action) => OnReceivedDamage.AddListener(element, action);
    public void RemoveOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action) => OnReceivedDamage.RemoveListener(element, action);

    /// <summary>
    /// 当受到此元素类型附着时调用的函数
    /// </summary>
    private ElementEvent OnAddElement = new ElementEvent();

    public void AddOnAddElementListener(Elements element, System.Action action) => OnAddElement.AddListener(element, action);
    public void RemoveOnAddElementListener(Elements element, System.Action action) => OnAddElement.RemoveListener(element, action);
    
    /// <summary>
    /// 元素附着被反应掉时调用的函数
    /// </summary>
    private ElementEvent<ElementsReaction> OnElementReacted = new ElementEvent<ElementsReaction>();

    public void AddOnElementReactedListener(Elements element, System.Action<ElementsReaction> action) => OnElementReacted.AddListener(element, action);
    public void RemoveOnElementReactedListener(Elements element, System.Action<ElementsReaction> action) => OnElementReacted.RemoveListener(element, action);


    protected int strength;
    public int Strength { get => strength; set => strength = value; }

    protected int speed;
    public int Speed { get => speed; set =>speed = value; }

    protected int health;
    public int Health { get => health; set => health = value; }

    protected int atkPower;
    public int AtkPower { get => atkPower; set => atkPower = value; }

    public GameObject GameObject { get; set; }
    public abstract GameObject OriginalReference { get; }

    public float GetResistance(Elements element) => resistances[element];
    public void SetResistance(float value, Elements element) => resistances[element] = value;

    public void AddEffect(IEffect effect) => effects.Add(effect);
    public void RemoveEffect(IEffect effect) => effects.Remove(effect);
    public List<IEffect> GetEffects() => effects;

    public virtual void OnAwake() { }

    public virtual void OnDestroy() { }

    public void ReceiveDamage(IElementalDamage damage)
    {
        //本地函数：没有反应，仅仅考虑抗性和附着
        void PlainDealsDamage(IElementalDamage damage)
        {
            //为元素伤害且能附着元素
            if(damage.CanAddElement) //能添加元素
            {
                //物理伤害、风元素伤害、岩元素伤害不能添加元素附着
                if(damage.ElementType != Elements.None && damage.ElementType != Elements.Wind && damage.ElementType != Elements.Stone)
                {
                    OnAddElement.Trigger(damage.ElementType);//触发添加元素逻辑
                    elements[damage.ElementType] = true;
                }
            }
            OnReceivedDamage.Trigger(damage.ElementType,damage);//触发受到伤害逻辑

            float value = GetResistance(damage.ElementType);
            health -= (int)(damage.AtkDmg * (1 - value));
        }
        //为元素伤害而且能附着元素
        if (damage.ElementType != Elements.None && damage.CanAddElement)//物理伤害不能添加元素
        {
            Elements causerElement = damage.ElementType;
            
            foreach (var item in elements)
            {
                if (item.Value == false)//没有这个元素
                    continue;
                Elements element = item.Key;
                ElementsReaction reaction = ElementsReaction.GetReaction(element, causerElement);//尝试找元素反应
                Debug.Log("Reaction:" + reaction.ReactionName);
                if (reaction != null)//存在元素反应，让其发生
                {
                    OnElementReacted.Trigger(element,reaction);//触发原来元素的反应逻辑
                    OnElementReacted.Trigger(causerElement, reaction);//触发新添加元素的反应逻辑

                    Debug.Log("ReactionHappened");
                    reaction.Action(damage, this);
                    
                    elements[element] = false;
                    damage.CanAddElement = false;//已经触发完反应，因此此次攻击不再附着元素
                    break;
                }
            }
        }
        PlainDealsDamage(damage);
    }


    public void AddElement(Elements element) => elements[element] = true;

    public void RemoveElement(Elements element) => elements[element] = false;
    
    public Elements[] GetAllElements()
    {
        List<Elements> total = new List<Elements>();
        foreach(var item in elements)
        {
            if (item.Value == true)
                total.Add(item.Key);
        }
        return total.ToArray();
    }


}
