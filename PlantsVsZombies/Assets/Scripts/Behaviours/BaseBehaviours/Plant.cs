using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 植物脚本基类
/// </summary>
public abstract class Plant : BaseGameobject
{
    public IPlantData Data { get; set; }
    public UnityAction<Plant> OnDie;
    protected virtual void OnDestroy()
    {
        OnDie?.Invoke(this);
    }
}
