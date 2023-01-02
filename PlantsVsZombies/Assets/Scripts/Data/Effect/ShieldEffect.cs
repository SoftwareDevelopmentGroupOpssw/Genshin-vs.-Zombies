using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldEffect : CountDownEffect
{
    private int shieldPower;
    private int duration;
    private IGameobjectData caster;
    private UnityAction callback;
    /// <summary>
    /// ���ɻ���Ч��
    /// </summary>
    /// <param name="shieldPower">����ֵ</param>
    /// <param name="milisecondsDuration">���ܳ���ʱ��</param>
    /// <param name="caster">�����ͷ���</param>
    /// <param name="callback">���ܽ���ʱ�ص�����</param>
    public ShieldEffect(int shieldPower, int milisecondsDuration, IGameobjectData caster, UnityAction callback = null)
    {
        this.shieldPower = shieldPower;
        this.duration = milisecondsDuration;
        this.caster = caster;
        this.callback = callback;
    }

    public override int MilisecondsDuration => duration;

    public override string EffectName => "ShieldEffect";
    
    private void OnReceiveDamageListener(IElementalDamage damage)
    {
        shieldPower -= damage.Damage;
        damage.Damage = 0;
    }

    public override IGameobjectData Caster => caster;
    public override void EnableEffect(IGameobjectData target)
    {
        if(!(target is IDamageReceiver))
        {
            State = EffectState.Error;
            Debug.LogError("Shield effect can only be added to damage receiver data.");
            return;
        }
        else
        {
            IDamageReceiver receiver = target as IDamageReceiver;
            receiver.AddOnReceiveAllDamageListener(OnReceiveDamageListener);
            State = EffectState.Processing;
            Start();
        }

    }
    public override void UpdateEffect(IGameobjectData target)
    {
        if (shieldPower <= 0)
            End();
        //��ʱ�����ʱ����CountDownEffect���Զ�����End����
    }
    public override void DisableEffect(IGameobjectData target)
    {
        callback?.Invoke();
        (target as IDamageReceiver).RemoveOnReceiveAllDamageListener(OnReceiveDamageListener);
    }
}
