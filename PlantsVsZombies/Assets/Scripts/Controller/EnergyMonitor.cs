using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 能量的类型
/// </summary>
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
/// <summary>
/// 能量管理模块
/// </summary>
public class EnergyMonitor
{
    private const string ENERGY_PREFAB_PATH = "Prefabs/UI/UIElements/";
    private int energyValue;//能量值
    private static GameObject smallEnergy;
    private static GameObject bigEnergy;
    private static List<GameObject> energyList = new List<GameObject>();
    public static void ForeachEnergy(System.Action<Energy> action)
    {
        energyList.ForEach((obj) =>
        {
            action.Invoke(obj.GetComponent<Energy>());
        });
    }
    /// <summary>
    /// 在指定的屏幕位置生成一个可以点击的能量，点击后自动加能量
    /// </summary>
    /// <param name="pixelPos">能量在屏幕中出现的位置，以左下角为原点，向右为x正，向上为y正</param>
    /// <param name="type">能量类型</param>
    public static void InstantiateEnergy(Vector2Int pixelPos, EnergyType type)
    {
        void LogError(string name)
        {
            Debug.LogError("Prefab:" + name + " is missing. It supposed to be at Resources/" + ENERGY_PREFAB_PATH + name);
        }

        GameObject obj = null;
        switch (type)
        {
            case EnergyType.Big:
                if (bigEnergy == null)
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
                if (smallEnergy == null)
                    smallEnergy = ResourceManager.Instance.Load<GameObject>(ENERGY_PREFAB_PATH + "SmallEnergy");
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
        //anchoredPostion以屏幕中心点为原点，而传入的pixelPos是以左下角为原点（也是Input.mousePosition的坐标）
        Vector2 location = new Vector2(pixelPos.x - Screen.width / 2, pixelPos.y - Screen.height / 2);
        rect.anchoredPosition = location;
        energyList.Add(energy);
    }
    public static void DestroyEnergy(Energy energy)
    {
        energyList.Remove(energy.gameObject);
        GameObject.Destroy(energy.gameObject);
    }
    public static void Clear()
    {
        foreach(var energy in energyList)
        {
            GameObject.Destroy(energy.gameObject);
        }
        energyList.Clear();
    }
    /// <summary>
    /// 能量值
    /// </summary>
    public int Energy 
    { 
        get => energyValue; 
        set
        {
            if (value >= 0)
                energyValue = value;
        } 
    }
    
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
    /// 减少能量
    /// </summary>
    /// <param name="value">能量</param>
    public void RemoveEnergy(int value)
    {
        energyValue -= value;
        OnValueChanged?.Invoke(energyValue);
    }
}
