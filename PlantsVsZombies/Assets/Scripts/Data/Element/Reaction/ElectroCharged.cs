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
    private static int damageDealtPerTime = 3;//一次感电造成伤害值
    private static int damageSpaceTime = 500;//两次感电伤害之间的间隔时间（毫秒）
    private static int deltaStrength = -30;//对于怪物所造成的韧性衰减值
    private StrengthEffect strength;
    
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
        target.AddEffect(this);

        //范围检测:范围内的施加感电效果
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            if (collider.gameObject.tag == "Monster")
            {
                IDamageable collideTarget = collider.gameObject.GetComponent<IDamageable>();//获取可以触发感电的对象
                //可以受到伤害
                if(collideTarget != null)
                {
                    //获取一个伤害接收器
                    IDamageReceiver receiver = collideTarget.GetReceiver();
                    MonoManager.Instance.StartCoroutine(DamageCoroutine(receiver));//对范围内的敌人造成伤害，但不附加元素
                }
            }
        }
    }
    /// <summary>
    /// 造成持续伤害的协程
    /// </summary>
    private Coroutine damageCoroutine;
    private IEnumerator DamageCoroutine(IDamageReceiver target)
    {
        for (int i = 0; i< damageTimes; i++)
        {
            target.ReceiveDamage(new SystemDamage(damageDealtPerTime, Elements.Electric));
            yield return new WaitForSecondsRealtime((float)damageSpaceTime / 1000);
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
        damageCoroutine = MonoManager.Instance.StartCoroutine(DamageCoroutine(this.target as IMonsterData));
        State = EffectState.Processing;
        //添加水雷共存
        this.target.AddElement(Elements.Water);
        this.target.AddElement(Elements.Electric);
        //添加元素反应监听
        this.target.AddOnElementReactedListener(Elements.Water, ElectroCharged_OnElementReacted);
        this.target.AddOnElementReactedListener(Elements.Electric, ElectroCharged_OnElementReacted);
        strength.EnableEffect(this.target);//开启韧性削减效果
    }

    public void DisableEffect(IGameobjectData target)
    {
        strength.DisableEffect(this.target);
        
        //被移除时移除水雷元素
        this.target.RemoveElement(Elements.Electric);
        this.target.RemoveElement(Elements.Water);
        //移除监听
        this.target.RemoveOnElementReactedListener(Elements.Water, ElectroCharged_OnElementReacted);
        this.target.RemoveOnElementReactedListener(Elements.Electric, ElectroCharged_OnElementReacted);
        
        MonoManager.Instance.StopCoroutine(damageCoroutine);//被移除时停止伤害协程
    }

    public void UpdateEffect(IGameobjectData target)
    {
        
    }
}
