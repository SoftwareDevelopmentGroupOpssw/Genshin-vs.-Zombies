using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 感电，既是一种反应又是一种效果
/// </summary>
public class ElectroCharged : ElementsReaction, IEffect
{
    private static float radius = 1.2f;   //感电伤害的半径
    private static int damageTimes = 4;    //感电造成伤害次数
    private static int damageDealtPerTime = 5;//一次感电造成伤害值
    private static int damageSpaceTime = 500;//两次感电伤害之间的间隔时间（毫秒）
    private static int deltaStrength = -30;//对于怪物所造成的韧性衰减值
    private StrengthEffect strength;
    /// <summary>
    /// 这个感电效果能否添加元素
    /// </summary>
    private bool canAddElement = true;

    private IDamageReceiver target;//触发感电反应的目标
    public StrengthEffect Strength => strength;
    public ElectroCharged()
    {
        strength = new StrengthEffect(deltaStrength, damageTimes * damageSpaceTime, system);
        State = EffectState.Initialized;
    }

    public override string ReactionName => Name;
    public static string Name => "Electro-Charged";

    public string EffectName => Name;

    public EffectState State { get; private set; }
    public IGameobjectData Caster => system;

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        this.target = target;
        List<IEffect> targetEffects = target.GetEffects();
        bool targetHavePrevious = false;
        if(targetEffects != null)
        {
            targetEffects.ForEach((effect) =>
            {
                if(effect is ElectroCharged)
                {
                    ElectroCharged electroCharged = effect as ElectroCharged;
                    if(electroCharged.canAddElement == true)
                    {
                        electroCharged.nowTimes = 0; //刷新时间
                        targetHavePrevious = true;//已经有一个能加元素的感电效果了
                        return;
                    }
                }
            });
        }
        if(!targetHavePrevious)
            target.AddEffect(this);//为主要目标加一个能添加元素的感电效果

        //范围检测:范围内的施加感电效果
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();//获取可以触发感电的对象
            //可以受到伤害
            if (damageable != null && damageable is Monster)
            {
                //获取一个伤害接收器
                IDamageReceiver receiver = damageable.GetReceiver();
                List<IEffect> effects = receiver.GetEffects();
                bool havePrevious = false;
                if(effects != null)
                {
                    effects.ForEach((effect) =>
                    {
                        if (effect is ElectroCharged)
                        {
                            //刷新身上的不加元素的感电效果
                            ElectroCharged electroCharged = effect as ElectroCharged;
                            if (!electroCharged.canAddElement)
                            {
                                electroCharged.nowTimes = 0;
                                havePrevious = true;
                                return;
                            }
                        }
                    });
                }
                if(!havePrevious)
                    //为其加上不加元素的感电效果
                    receiver.AddEffect(new ElectroCharged() { target = receiver, canAddElement = false });
            }
        }
    }
    /// <summary>
    /// 造成持续伤害的协程
    /// </summary>
    private Coroutine damageCoroutine;
    private int nowTimes = 0;
    private IEnumerator DamageCoroutine(IDamageReceiver target)
    {
        for (;nowTimes < damageTimes; nowTimes++)
        {
            target.ReceiveDamage(new SystemDamage(damageDealtPerTime, Elements.Electric));
            yield return new WaitForSecondsRealtime(damageSpaceTime / 1000f);
        }
        State = EffectState.End;
    }
    /// <summary>
    /// 当水雷元素被反应掉时，无论新反应是感电还是其它反应，都将原来的感电反应和水雷元素移除
    /// </summary>
    /// <param name="reaction"></param>
    void ElectroCharged_OnElementReacted(ElementsReaction reaction)
    {
        State = EffectState.End;//感电效果结束
        target.RemoveOnElementReactedListener(Elements.Water, ElectroCharged_OnElementReacted);
        target.RemoveOnElementReactedListener(Elements.Electric, ElectroCharged_OnElementReacted);
    }
    public void EnableEffect(IGameobjectData target)
    {
        //开启持续伤害的协程
        damageCoroutine = MonoManager.Instance.StartCoroutine(DamageCoroutine(this.target));
        State = EffectState.Processing;
        
        if (canAddElement)
        {
            //添加水雷共存
            this.target.AddElement(Elements.Electric);
            this.target.AddElement(Elements.Water);
            //添加元素反应监听
            this.target.AddOnElementReactedListener(Elements.Water, ElectroCharged_OnElementReacted);
            this.target.AddOnElementReactedListener(Elements.Electric, ElectroCharged_OnElementReacted);
        }
        
        strength.EnableEffect(this.target);//开启韧性削减效果
    }

    public void DisableEffect(IGameobjectData target)
    {

        strength.DisableEffect(this.target);

        if (canAddElement)
        {
            //被移除时移除水雷元素
            this.target.RemoveElement(Elements.Electric);
            this.target.RemoveElement(Elements.Water);
            //移除监听
            this.target.RemoveOnElementReactedListener(Elements.Water, ElectroCharged_OnElementReacted);
            this.target.RemoveOnElementReactedListener(Elements.Electric, ElectroCharged_OnElementReacted);
        }
        MonoManager.Instance.StopCoroutine(damageCoroutine);//被移除时停止伤害协程
    }

    public void UpdateEffect(IGameobjectData target)
    {
        
    }
}
