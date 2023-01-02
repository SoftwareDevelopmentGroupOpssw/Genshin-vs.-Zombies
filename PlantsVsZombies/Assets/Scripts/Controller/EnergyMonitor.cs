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
    /// �������buffer������
    /// </summary>
    private readonly static string bufferName = "Energy";
    /// <summary>
    /// ��ȡԭʼԤ����
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
    /// ��ָ������Ļλ������һ�����Ե����������������Զ�������
    /// </summary>
    /// <param name="pixelPos">��������Ļ�г��ֵ�λ�ã������½�Ϊԭ�㣬����Ϊx��������Ϊy��</param>
    /// <param name="type">��������</param>
    public static GameObject CreateEnergy(Vector2Int pixelPos, EnergyType type)
    {
        //����й�
        EntitiesController controller = GameController.Instance.EntitiesController;
        if (!controller.ContainsBuffer(bufferName))
        {
            ObjectBuffer buffer = new ObjectBuffer(UIManager.Instance.GetUILayer(UIManager.UILayer.Bot));
            controller.AddBufferToManagement(bufferName,buffer);
            controller.AddBehaviourManagement<Energy>(bufferName, BigEnergy);
            controller.AddBehaviourManagement<Energy>(bufferName, SmallEnergy);
        }
        //ԭʼ����
        GameObject originalSource = GetEnergySource(type);
        //ʵ����
        GameObject energy = controller.Get(bufferName, originalSource);
        energy.GetComponent<Energy>().EnerygyType = type;

        RectTransform rect = energy.transform as RectTransform;
        //anchoredPostion����Ļ���ĵ�Ϊԭ�㣬�������pixelPos�������½�Ϊԭ�㣨Ҳ��Input.mousePosition�����꣩
        Vector2 location = new Vector2(pixelPos.x - Screen.width / 2, pixelPos.y - Screen.height / 2);
        rect.anchoredPosition = location;

        return energy;
    }

    public static void DestroyEnergy(Energy energy)
    {
        GameObject source = GetEnergySource(energy.EnerygyType);
        GameController.Instance.EntitiesController.Put(bufferName, source, energy.gameObject);
    }


    private int energyValue;//����ֵ
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
