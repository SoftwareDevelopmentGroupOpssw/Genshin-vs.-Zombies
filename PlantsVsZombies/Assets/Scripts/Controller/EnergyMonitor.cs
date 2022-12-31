using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ����������
/// </summary>
public enum EnergyType
{
    /// <summary>
    /// ����������25������
    /// </summary>
    Big,
    /// <summary>
    /// С��������15������
    /// </summary>
    Small,
}
/// <summary>
/// ��������ģ��
/// </summary>
public class EnergyMonitor
{
    private const string ENERGY_PREFAB_PATH = "Prefabs/UI/UIElements/";
    private int energyValue;//����ֵ
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
    /// ��ָ������Ļλ������һ�����Ե����������������Զ�������
    /// </summary>
    /// <param name="pixelPos">��������Ļ�г��ֵ�λ�ã������½�Ϊԭ�㣬����Ϊx��������Ϊy��</param>
    /// <param name="type">��������</param>
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
        //anchoredPostion����Ļ���ĵ�Ϊԭ�㣬�������pixelPos�������½�Ϊԭ�㣨Ҳ��Input.mousePosition�����꣩
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
    /// ����ֵ
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
    /// ������ֵ�ı�ʱ�����õ��¼��㲥
    /// </summary>
    public event UnityAction<int> OnValueChanged;
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="value">����ֵ</param>
    public void AddEnergy(int value)
    {
        energyValue += value;
        OnValueChanged?.Invoke(energyValue);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="value">����</param>
    public void RemoveEnergy(int value)
    {
        energyValue -= value;
        OnValueChanged?.Invoke(energyValue);
    }
}
