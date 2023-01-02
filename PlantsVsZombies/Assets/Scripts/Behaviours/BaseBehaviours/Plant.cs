using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ֲ��ű�����
/// </summary>
public abstract class Plant : BaseGameobject, IDamageable
{
    /// <summary>
    /// ֲ��������Ϣ
    /// </summary>
    public IPlantData Data { get; set; }
    /// <summary>
    /// ֲ��Ч��������
    /// </summary>
    public abstract IEffectHandler Handler { get; }
    /// <summary>
    /// ���ֲ���˺�������
    /// </summary>
    /// <returns></returns>
    public IDamageReceiver GetReceiver() => Data;
    protected virtual void Update()
    {
        Handler.CheckEffect();
        if (Data.Health <= 0)
        {
            OnDead();
            GameController.Instance.PlantsController.RemovePlant(this);
            Handler.DisableAll();
            return;
        }
    }
    /// <summary>
    /// ���������Ƴ�ǰ����
    /// </summary>
    protected virtual void OnDead()
    {

    }

    public ICharactorData GetData()
    {
        return Data;
    }
}
