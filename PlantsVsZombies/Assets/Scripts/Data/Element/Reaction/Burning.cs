using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 燃烧
/// </summary>
public class Burning : ElementsReaction,IEffect
{
    private static int damageTimes = 8;    //燃烧造成伤害次数
    private static int damageDealtPerTime = 4;//一次燃烧造成伤害值
    private static int damageSpaceTime = 1000;//两次燃烧伤害之间的间隔时间（毫秒）

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
        target.AddEffect(this);
    }
    /// <summary>
    /// 持续造成伤害
    /// </summary>
    /// <param name="monster"></param>
    /// <returns></returns>
    private IEnumerator DamageCoroutine(IDamageReceiver monster)
    {
        for (int i = 0; i < damageTimes; i++)
        {
            monster.ReceiveDamage(new SystemDamage(damageDealtPerTime, Elements.Fire));
            yield return new WaitForSecondsRealtime((float)damageSpaceTime / 1000);
        }
        State = EffectState.End;
    }

    public void DisableEffect(IGameobjectData target)
    {
        
    }


    public void EnableEffect(IGameobjectData target)
    {
        MonoManager.Instance.StartCoroutine(DamageCoroutine(this.target));
        State = EffectState.Processing;
    }

    public void UpdateEffect(IGameobjectData target)
    {
        this.target.ReceiveDamage(new SystemDamage(0, Elements.Fire, true));//持续附着火元素
    }
}
