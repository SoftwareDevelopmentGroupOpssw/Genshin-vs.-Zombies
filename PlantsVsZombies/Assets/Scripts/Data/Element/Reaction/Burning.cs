using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 燃烧
/// </summary>
public class Burning : ElementsReaction,IEffect
{
    private static int damageTimes = 8;    //燃烧造成伤害次数
    private static int damageDealtPerTime = 3;//一次燃烧造成伤害值
    private static int damageSpaceTime = 300;//两次燃烧伤害之间的间隔时间（毫秒）

    private IDamageReceiver target;//魔物对象

    public Burning()
    {
        State = EffectState.Initialized;
    }

    public override string ReactionName => "Burning";

    public string EffectName => ReactionName;

    public EffectState State { get; private set; }

    public IGameobjectData Caster => system;

    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        this.target = target;
        List<IEffect> effects = target.GetEffects();
        bool havePrevious = false;//是否有先前的效果
        if(effects != null)
            
            effects.ForEach(effect =>
            {
                if (effect is Burning)
                {
                    //刷新先前的燃烧状态
                    (effect as Burning).nowTimes = 0;
                    havePrevious = true;
                    return;
                }
            });
        if(!havePrevious)//没有先前的效果就给它再添加一个
            target.AddEffect(this);
    }
    private Coroutine damageCoroutine;
    /// <summary>
    /// 目前造成伤害的次数
    /// </summary>
    private int nowTimes = 0;
    /// <summary>
    /// 持续造成伤害
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    private IEnumerator DamageCoroutine(IDamageReceiver monster)
    {
        for (; nowTimes < damageTimes; nowTimes++)
        {
            monster.ReceiveDamage(new SystemDamage(damageDealtPerTime, Elements.Fire, true));
            yield return new WaitForSecondsRealtime((float)damageSpaceTime / 1000);
        }
        State = EffectState.End;
    }

    public void DisableEffect(IGameobjectData target)
    {
        MonoManager.Instance.StopCoroutine(damageCoroutine);
    }


    public void EnableEffect(IGameobjectData target)
    {
        damageCoroutine = MonoManager.Instance.StartCoroutine(DamageCoroutine(this.target));
        State = EffectState.Processing;
    }

    public void UpdateEffect(IGameobjectData target)
    {

    }
}
