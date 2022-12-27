using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����
/// </summary>
public class Quicken : ElementsReaction,IEffect
{
    private static int electicDamageChange = 15;//��Ԫ���˺����ֵ
    private static int grassDamageChange = 15;//��Ԫ���˺����ֵ
    private static int duration = 6000;//�˺���߳���ʱ�䣨���룩
    private CountDown countDown;
    private IMonsterData target;//��������
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

    public override void Action(IElementalDamage damage, IMonsterData target)
    {
        this.target = target;
        target.AddEffect(this);
    }
    /// <summary>
    /// �ܵ��˺�ʱ�����˺�
    /// </summary>
    /// <param name="damage"></param>
    void DamageChange(IElementalDamage damage)
    {
        if (damage.ElementType == Elements.Electric)
            damage.AtkDmg += electicDamageChange;
        else if (damage.ElementType == Elements.Grass)
            damage.AtkDmg += grassDamageChange;
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
