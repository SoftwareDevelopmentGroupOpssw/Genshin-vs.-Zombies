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
    private static GameObject smallEnergy;
    private static GameObject SmallEnergy
    {
        get
        {
            if (smallEnergy == null)
                smallEnergy = ResourceManager.Instance.Load<GameObject>(ENERGY_PREFAB_PATH + "SmallEnergy");
            return smallEnergy;
        }
    }
    private static GameObject bigEnergy;
    private static GameObject BigEnergy
    {
        get
        {
            if (bigEnergy == null)
                bigEnergy = ResourceManager.Instance.Load<GameObject>(ENERGY_PREFAB_PATH + "BigEnergy");
            return bigEnergy;
        }
    }
    /// <summary>
    /// 被管理的buffer的名字
    /// </summary>
    private readonly static string bufferName = "Energy";
    /// <summary>
    /// 获取原始预制体
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static GameObject GetEnergySource(EnergyType type)
    {
        GameObject originalSource = null;
        switch (type)
        {
            case EnergyType.Big:
                originalSource = BigEnergy;
                break;
            case EnergyType.Small:
                originalSource = SmallEnergy;
                break;
        }
        return originalSource;
    }
    /// <summary>
    /// 在指定的屏幕位置生成一个可以点击的能量，点击后自动加能量
    /// </summary>
    /// <param name="pixelPos">能量在屏幕中出现的位置，以左下角为原点，向右为x正，向上为y正</param>
    /// <param name="type">能量类型</param>
    public static GameObject CreateEnergy(Vector2Int pixelPos, EnergyType type)
    {
        //添加托管
        EntitiesController controller = GameController.Instance.EntitiesController;
        if (!controller.ContainsBuffer(bufferName))
        {
            ObjectBuffer buffer = new ObjectBuffer(UIManager.Instance.GetUILayer(UIManager.UILayer.Bot));
            controller.AddBufferToManagement(bufferName,buffer);
            controller.AddBehaviourManagement<Energy>(bufferName, BigEnergy);
            controller.AddBehaviourManagement<Energy>(bufferName, SmallEnergy);
        }
        //原始对象
        GameObject originalSource = GetEnergySource(type);
        //实例化
        GameObject energy = controller.Get(bufferName, originalSource);
        energy.GetComponent<Energy>().EnerygyType = type;

        RectTransform rect = energy.transform as RectTransform;
        //anchoredPostion以屏幕中心点为原点，而传入的pixelPos是以左下角为原点（也是Input.mousePosition的坐标）
        Vector2 location = new Vector2(pixelPos.x - Screen.width / 2, pixelPos.y - Screen.height / 2);
        rect.anchoredPosition = location;

        return energy;
    }

    public static void DestroyEnergy(Energy energy)
    {
        GameObject source = GetEnergySource(energy.EnerygyType);
        GameController.Instance.EntitiesController.Put(bufferName, source, energy.gameObject);
    }


    private int energyValue;//能量值
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
