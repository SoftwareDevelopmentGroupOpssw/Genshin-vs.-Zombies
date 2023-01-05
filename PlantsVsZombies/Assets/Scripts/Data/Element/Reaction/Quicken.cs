using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 激化
/// </summary>
public class Quicken : ElementsReaction,IEffect
{
    private static int electicDamageChange = 15;//雷元素伤害提高值
    private static int grassDamageChange = 15;//草元素伤害提高值
    private static int duration = 6000;//伤害提高持续时间（毫秒）
    private CountDown countDown;
    private IDamageReceiver target;//触发对象
    public Quicken()
    {
        countDown = new CountDown(duration);
        countDown.OnComplete += End;

        State = EffectState.Initialized;
    }


    public override string ReactionName => "Quicken";

    public string EffectName => ReactionName;

    public EffectState State { get; private set; }

    public IGameobjectData Caster => system;

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        this.target = target;
        //先前是否已经被添加激化效果
        IEffect effect = target.GetEffects().Find((effect) => effect is Quicken);
        if(effect == null) //没有激化效果才再次添加激化效果
            target.AddEffect(this);
    }
    /// <summary>
    /// 受到伤害时增加伤害
    /// </summary>
    /// <param name="damage"></param>
    void DamageChange(IElementalDamage damage)
    {
        if (damage.ElementType == Elements.Electric)
            damage.Damage += electicDamageChange;
        else if (damage.ElementType == Elements.Grass)
            damage.Damage += grassDamageChange;
    }

    public void DisableEffect(IGameobjectData target)
    {
        this.target.RemoveOnReceiveDamageListener(Elements.Electric, DamageChange);
        this.target.RemoveOnReceiveDamageListener(Elements.Grass, DamageChange);
    }

    public void EnableEffect(IGameobjectData target)
    {
        this.target.AddOnReceiveDamageListener(Elements.Electric, DamageChange);
        this.target.AddOnReceiveDamageListener(Elements.Grass, DamageChange);

        countDown.StartCountDown();
        State = EffectState.Processing;
    }
    void End() => State = EffectState.End;
    public void UpdateEffect(IGameobjectData target)
    {
        
    }
}
