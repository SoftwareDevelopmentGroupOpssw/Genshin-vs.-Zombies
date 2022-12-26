using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterData : IMonsterData
{
    /// <summary>
    /// 身上的效果
    /// </summary>
    private List<IEffect> effects = new List<IEffect>();
    
    /// <summary>
    /// 附着的元素
    /// </summary>
    private bool[] elements = new bool[8];
    
    /// <summary>
    /// 抗性值
    /// </summary>
    private float[] resistances = new float[8];

    /// <summary>
    /// 在此次Update调用中，是否还能进行Action操作
    /// 如果受到了眩晕效果，就不能进行Action操作
    /// </summary>
    private bool canAction = true;

    protected int strength;
    public int Strength { get => strength; }
    
    protected int speed;
    public int Speed { get => speed; }
    
    protected int health;
    public int Health { get => health; }

    protected int atkPower;
    public int AtkPower { get => atkPower; }

    public GameObject GameObject { get; set; }
    public abstract string ResourcePath { get; }

    public float GetResistance(Elements element) => resistances[(int)element];

    protected float SetResistance(float value, Elements element) => resistances[(int)element] = value;

    public void AddEffect(IEffect effect) => effects.Add(effect);

    public abstract IGameobjectData Instantiate();

    public virtual void OnAwake() { }

    public virtual void OnDestroy() { }

    public void ReceiveDamage(IElementalDamage damage)
    {
        //本地函数：造成伤害
        void PlainDealsDamage(IElementalDamage damage)
        {
            float value = GetResistance(damage.ElementType);
            health -= (int)(damage.AtkDmg * (1-value));
            if(damage.ElementType != Elements.None && damage.CanAddElement)
                elements[(int)damage.ElementType] = true;
        }
        if (damage.ElementType != Elements.None && damage.CanAddElement)
        {
            Elements causerElement = damage.ElementType;
            int i = 0;
            for (; i < elements.Length; i++)
            {
                if (!elements[i])
                    continue;
                Elements element = (Elements)i;
                ElementsReaction reaction = ElementsReaction.GetReaction(element, causerElement);
                if (reaction != null)
                {
                    reaction.Action(damage, this);
                    elements[i] = false;
                    break;
                }
            }
            if (i == elements.Length)//没有找到能够反应的元素
                PlainDealsDamage(damage);
        }
        else
            PlainDealsDamage(damage);
        
    }

    protected void AddElement(Elements element)
    {
        elements[(int)element] = true;
    }

    protected void RemoveElement(Elements element)
    {
        elements[(int)element] = false;
    }
    protected Elements[] GetAllElements()
    {
        List<Elements> total = new List<Elements>();
        for(int i = 0; i < elements.Length; i++)
        {
            if (elements[i])
                total.Add((Elements)i);
        }
        return total.ToArray();
    }
    /// <summary>
    /// 经过触发所有效果后，最终真实的操作
    /// </summary>
    protected abstract void RealAction();
    /// <summary>
    /// 启用所有效果
    /// </summary>
    private void EnableAllEffect()
    {
        List<int> deletes = new List<int>();
        for(int i = 0;i < effects.Count; i++)
        {
            IEffect effect = effects[i];
            switch (effect.State)
            {
                case EffectState.Initialized:
                    EnableEffect(effect);
                    break;
                case EffectState.Processing:
                    UpdateEffect(effect);
                    break;
                case EffectState.End:
                    deletes.Add(i);
                    continue;
            }
        }
        foreach(int index in deletes)
        {
            IEffect effect = effects[index];
            effects.RemoveAt(index);
            DisableEffect(effect);
        }
    }
    /// <summary>
    /// 自定义启用单个效果
    /// </summary>
    /// <param name="effect">效果</param>
    private void EnableEffect(IEffect effect)
    {
        if (effect is SpeedEffect)
            (effect as SpeedEffect).EnableEffect(ref speed);
        else if(effect is StunEffect)
        {
            canAction = false;
            (effect as StunEffect).EnableEffect();
        }
    }
    /// <summary>
    /// 自定义在效果的持续时间里更新效果
    /// </summary>
    /// <param name="effect"></param>
    private void UpdateEffect(IEffect effect)
    {
        if (effect is StunEffect)//如果有眩晕效果，每一帧都不能进行操作
            canAction = false;
    }
    /// <summary>
    ///自定义停止单个效果
    /// </summary>
    /// <param name="effect">效果</param>
    private void DisableEffect(IEffect effect)
    {
        if(effect is SpeedEffect)
            (effect as SpeedEffect).DisableEffect(ref speed);
    }
    public void Action()
    {
        EnableAllEffect();
        if (canAction)//没有收到效果，可以继续操作
            RealAction();
        else
            canAction = true;//重新调整，下一帧再判断
    }
}
