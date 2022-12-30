using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ȼ��
/// </summary>
public class Burning : ElementsReaction,IEffect
{
    private static int damageTimes = 8;    //ȼ������˺�����
    private static int damageDealtPerTime = 4;//һ��ȼ������˺�ֵ
    private static int damageSpaceTime = 1000;//����ȼ���˺�֮��ļ��ʱ�䣨���룩

    private IDamageReceiver target;//ħ�����

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
    /// ��������˺�
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
        this.target.ReceiveDamage(new SystemDamage(0, Elements.Fire, true));//�������Ż�Ԫ��
    }
}
