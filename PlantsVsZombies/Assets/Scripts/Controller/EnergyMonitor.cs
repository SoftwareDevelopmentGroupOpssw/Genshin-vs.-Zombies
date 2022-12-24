using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnergyMonitor
{
    private int energyValue;
    /// <summary>
    /// 能量值 大于0
    /// </summary>
    public int Energy => energyValue;
    /// <summary>
    /// 当能量值改变时，调用的事件广播
    /// </summary>
    public event UnityAction<int> OnValueChanged;
    /// <summary>
    /// 增加能量
    /// </summary>
    /// <param name="value">能量值</param>
    public void AddEnergy(int value)
    {
        energyValue += value;
        OnValueChanged?.Invoke(energyValue);
    }
    /// <summary>
    /// 尝试减少能量，若能量不足则不会减少并返回False
    /// </summary>
    /// <param name="value">能量</param>
    /// <returns>减少操作是否成功</returns>
    public bool TryRemoveEnergy(int value)
    {
        if(energyValue < value)
        {
            return false;
        }
        else
        {
            energyValue -= value;
            OnValueChanged?.Invoke(energyValue);
            return true;
        }
    }
}
