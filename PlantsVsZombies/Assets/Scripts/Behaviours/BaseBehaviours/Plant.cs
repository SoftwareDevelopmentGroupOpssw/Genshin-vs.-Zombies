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

    protected virtual void Update()
    {
        if (Data.Health <= 0)
        {
            GameController.Instance.PlantsController.RemovePlant(this);

            return;
        }
    }
 

    public ICharactorData GetData()
    {
        return Data;
    }
}
