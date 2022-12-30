using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 植物脚本基类
/// </summary>
public abstract class Plant : BaseGameobject,IMonsterAttackable
{
    /// <summary>
    /// 植物数据信息
    /// </summary>
    public IPlantData Data { get; set; }
    /// <summary>
    /// 死亡时调用的事件
    /// </summary>
    public event UnityAction<Plant> OnDie;
    protected virtual void Update()
    {
        if (Data.Health <= 0)
            GameController.Instance.PlantsController.RemovePlant(this);
    }
    protected virtual void OnDestroy()
    {
        OnDie?.Invoke(this);
    }

    public ICharactorData GetData()
    {
        return Data;
    }
}
