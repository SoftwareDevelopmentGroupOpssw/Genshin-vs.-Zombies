using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ȼ��
/// </summary>
public class Burning : ElementsReaction,IEffect
{
    private static int damageTimes = 8;    //ȼ������˺�����
    private static int damageDealtPerTime = 3;//һ��ȼ������˺�ֵ
    private static int damageSpaceTime = 300;//����ȼ���˺�֮��ļ��ʱ�䣨���룩

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
        List<IEffect> effects = target.GetEffects();
        bool havePrevious = false;//�Ƿ�����ǰ��Ч��
        if(effects != null)
            
            effects.ForEach(effect =>
            {
                if (effect is Burning)
                {
                    //ˢ����ǰ��ȼ��״̬
                    (effect as Burning).nowTimes = 0;
                    havePrevious = true;
                    return;
                }
            });
        if(!havePrevious)//û����ǰ��Ч���͸��������һ��
            target.AddEffect(this);
    }
    private Coroutine damageCoroutine;
    /// <summary>
    /// Ŀǰ����˺��Ĵ���
    /// </summary>
    private int nowTimes = 0;
    /// <summary>
    /// ��������˺�
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
