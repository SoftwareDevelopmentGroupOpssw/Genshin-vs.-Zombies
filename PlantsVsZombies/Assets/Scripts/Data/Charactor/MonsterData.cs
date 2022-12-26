using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class MonsterData : IMonsterData
{
    /// <summary>
    /// 身上的效果
    /// </summary>
    private List <IEffect> effects = new List<IEffect>();
    /// <summary>
    /// 在下一次帧更新时需要删除的效果
    /// </summary>
    private List<IEffect> removeList = new List<IEffect>();

    /// <summary>
    /// 附着的元素
    /// </summary>
    private bool[] elements = new bool[8];

    /// <summary>
    /// 当受到此元素类型伤害时调用的函数
    /// </summary>
    private UnityAction<IElementalDamage>[] OnReceivedDamage = new UnityAction<IElementalDamage>[8];
    /// <summary>
    /// 当受到此元素类型附着时调用的函数
    /// </summary>
    private UnityAction[] OnAddElement = new UnityAction[8];
    /// <summary>
    /// 元素附着被反应掉时调用的函数
    /// </summary>
    private UnityAction<ElementsReaction>[] OnElementReacted = new UnityAction<ElementsReaction>[8];

    /// <summary>
    /// 抗性值
    /// </summary>
    private float[] resistances = new float[8];

    /// <summary>
    /// 在此次Update调用中，是否还能进行Action操作
    /// 如果受到了眩晕效果，就不能进行Action操作
    /// </summary>
    private bool canAction = true;
    public bool CanAction => canAction;

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
    public void RemoveEffect(IEffect effect) => removeList.Add(effect);

    public abstract IGameobjectData Instantiate();

    public virtual void OnAwake() { }

    public virtual void OnDestroy() { }

    public void ReceiveDamage(IElementalDamage damage)
    {
        //本地函数：没有反应，仅仅考虑抗性和附着
        void PlainDealsDamage(IElementalDamage damage)
        {
            int elementNum = (int)damage.ElementType;
            //为元素伤害且能附着元素
            if(damage.ElementType != Elements.None && damage.CanAddElement) //物理伤害不能添加元素
            {
                OnAddElement[elementNum]?.Invoke();
                elements[elementNum] = true;
            }
            OnReceivedDamage[elementNum]?.Invoke(damage);

            float value = GetResistance(damage.ElementType);
            health -= (int)(damage.AtkDmg * (1 - value));
        }
        //为元素伤害而且能附着元素
        if (damage.ElementType != Elements.None && damage.CanAddElement)//物理伤害不能添加元素
        {
            Elements causerElement = damage.ElementType;
            
            int i = 0;
            for (; i < elements.Length; i++)
            {
                if (!elements[i])//没有这个元素
                    continue;
                Elements element = (Elements)i;
                ElementsReaction reaction = ElementsReaction.GetReaction(element, causerElement);//尝试找元素反应
                if (reaction != null)//存在元素反应，让其发生
                {
                    OnElementReacted[(int)element]?.Invoke(reaction);
                    OnElementReacted[(int)causerElement]?.Invoke(reaction);
                    
                    reaction.Action(damage, this);
                    
                    elements[i] = false;
                    damage.CanAddElement = false;//已经触发完反应，因此此次攻击不再附着元素
                    break;
                }
            }
        }
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
    /// 检查所有效果
    /// </summary>
    private void CheckAllEffect()
    {
        RemoveUselessEffect();
        for(int i = effects.Count - 1;i >= 0; i--)
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
                    RemoveEffect(effect);//将效果添加到待删除列表中
                    continue;
            }
        }
    }
    private void RemoveUselessEffect()
    {
        foreach(var effect in removeList)
        {
            effects.Remove(effect);
            DisableEffect(effect);
        }
        removeList.Clear();
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
        else if(effect is Frozen)//当特殊效果为冻结效果时
        {
            void FrozenEffect_OnIceReacted(ElementsReaction reaction)//受到冻结效果时 若冰元素附着消失 触发的逻辑
            {
                if (reaction.Name != "Frozen") //触发了别的元素反应
                {
                    RemoveEffect(effect);//强制移除冻结效果
                    OnElementReacted[(int)Elements.Ice] -= FrozenEffect_OnIceReacted;
                }
            }
            void FrozenEffect_OnReceivePhysicalDamage(IElementalDamage damage)//当在冻结情况下受到物理伤害时触发碎冰效果
            {
                RemoveEffect(effect);
                health -= Frozen.BROKE_ICE_DAMAGE;//直接减少生命值，造成真实伤害

                OnReceivedDamage[(int)Elements.None] -= FrozenEffect_OnReceivePhysicalDamage;
            }
            Frozen frozen = effect as Frozen;
            //眩晕和减速状态的叠加
            EnableEffect(frozen.Speed);
            EnableEffect(frozen.Stun);
            ReceiveDamage(new SystemDamage(0, Elements.Ice, true));//添加冰元素附着
            OnElementReacted[(int)Elements.Ice] += FrozenEffect_OnIceReacted;//添加元素附着监听
            OnReceivedDamage[(int)Elements.None] += FrozenEffect_OnReceivePhysicalDamage;//添加物理伤害监听
        }
    }

    /// <summary>
    /// 自定义在效果的持续时间里更新效果
    /// </summary>
    /// <param name="effect"></param>
    private void UpdateEffect(IEffect effect)
    {
        if (effect is StunEffect)
            canAction = false; //如果有眩晕效果，每一帧都不能进行操作
        else if(effect is Frozen)
        {
            Frozen frozen = effect as Frozen;
            if (frozen.Stun.State == EffectState.Processing)
                UpdateEffect(frozen.Stun);
        }
    }
    /// <summary>
    ///自定义停止单个效果
    /// </summary>
    /// <param name="effect">效果</param>
    private void DisableEffect(IEffect effect)
    {
        if (effect is SpeedEffect)
            (effect as SpeedEffect).DisableEffect(ref speed);
        else if (effect is Frozen)
        {
            DisableEffect((effect as Frozen).Speed);
        }
    }
    public void Action()
    {
        CheckAllEffect();
        if (canAction)//没有收到效果，可以继续操作
            RealAction();
        else
            canAction = true;//重新调整，下一帧再判断
    }
}
