using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ֲ��ű�����
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
