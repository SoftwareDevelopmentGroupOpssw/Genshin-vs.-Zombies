using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EnergyType
{
    /// <summary>
    /// 大能量产出25点能量
    /// </summary>
    Big,
    /// <summary>
    /// 小能量产出15点能量
    /// </summary>
    Small,
}
public class EnergyMonitor
{
    private const string ENERGY_PREFAB_PATH = "Prefabs/UI/UIElements/";
    private static GameObject smallEnergy;
    private static GameObject bigEnergy;
    /// <summary>
    /// 生成一个可以点击的能量，点击后自动加能量
    /// </summary>
    /// <param name="pixelPos">能量出现的位置</param>
    /// <param name="type">能量类型</param>
    public static void InstantiateEnergy(Vector2Int pixelPos, EnergyType type)
    {
        void LogError(string name)
        {   
            Debug.LogError("Prefab:"+ name + " is missing. It supposed to be at Resources/" + ENERGY_PREFAB_PATH + name);
        }
        
        GameObject obj = null;
        switch (type)
        {
            case EnergyType.Big:
                if(bigEnergy == null)
                    bigEnergy = ResourceManager.Instance.Load<GameObject>(ENERGY_PREFAB_PATH + "BigEnergy");
                if (bigEnergy == null)
                {
                    LogError("BigEnergy");
                    return;
                }
                else
                    obj = bigEnergy;
                break;
            case EnergyType.Small:
                if(smallEnergy == null)
                    smallEnergy = ResourceManager.Instance.Load<GameObject> (ENERGY_PREFAB_PATH + "SmallEnergy");
                if (smallEnergy == null)
                {
                    LogError("SmallEnergy");
                    return;
                }
                else
                    obj = smallEnergy;
                break;
        }
        GameObject energy = GameObject.Instantiate(obj, UIManager.Instance.GetUILayer(UIManager.UILayer.Bot));
        RectTransform rect = energy.transform as RectTransform;
        rect.anchoredPosition = new Vector2(pixelPos.x, pixelPos.y);
    }
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
