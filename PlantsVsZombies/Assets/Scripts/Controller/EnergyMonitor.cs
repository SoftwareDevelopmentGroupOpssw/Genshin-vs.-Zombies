using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
public class EnergyMonitor
{
    private const string ENERGY_PREFAB_PATH = "Prefabs/UI/UIElements/";
    private static GameObject smallEnergy;
    private static GameObject bigEnergy;
    /// <summary>
    /// ����һ�����Ե����������������Զ�������
    /// </summary>
    /// <param name="pixelPos">�������ֵ�λ��</param>
    /// <param name="type">��������</param>
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
    /// ����ֵ ����0
    /// </summary>
    public int Energy => energyValue;
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
    /// ���Լ��������������������򲻻���ٲ�����False
    /// </summary>
    /// <param name="value">����</param>
    /// <returns>���ٲ����Ƿ�ɹ�</returns>
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
