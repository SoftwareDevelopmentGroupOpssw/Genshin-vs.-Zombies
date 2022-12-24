using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnergyMonitor
{
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
