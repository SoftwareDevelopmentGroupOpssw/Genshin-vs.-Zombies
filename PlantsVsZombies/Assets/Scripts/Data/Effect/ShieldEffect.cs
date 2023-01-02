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
    /// 生成护盾效果
    /// </summary>
    /// <param name="shieldPower">护盾值</param>
    /// <param name="milisecondsDuration">护盾持续时间</param>
    /// <param name="caster">护盾释放者</param>
    /// <param name="callback">护盾结束时回调函数</param>
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
        //当时间结束时，由CountDownEffect会自动调用End函数
    }
    public override void DisableEffect(IGameobjectData target)
    {
        callback?.Invoke();
        (target as IDamageReceiver).RemoveOnReceiveAllDamageListener(OnReceiveDamageListener);
    }
}
