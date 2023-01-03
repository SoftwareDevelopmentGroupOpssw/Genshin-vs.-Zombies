using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 植物脚本基类
/// </summary>
public abstract class Plant : BaseGameobject, IDamageable
{
    /// <summary>
    /// 植物数据信息
    /// </summary>
    public IPlantData Data { get; set; }
    /// <summary>
    /// 植物效果处理器
    /// </summary>
    public abstract IEffectHandler Handler { get; }
    /// <summary>
    /// 获得植物伤害处理器
    /// </summary>
    /// <returns></returns>
    public IDamageReceiver GetReceiver() => Data;
    protected virtual void Update()
    {
        Handler.CheckEffect();
        if (Data.Health <= 0)
        {
            Handler.DisableAll();
            GameController.Instance.PlantsController.RemovePlant(this);
            return;
        }
    }

    public ICharactorData GetData()
    {
        return Data;
    }
}
