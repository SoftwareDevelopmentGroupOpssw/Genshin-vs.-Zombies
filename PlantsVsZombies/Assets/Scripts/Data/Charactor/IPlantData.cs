using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ֲ��ӿ�
/// </summary>
public interface IPlantData:ICharactorData
{
    /// <summary>
    /// ֲ���Ƿ�׼�����´��ж�
    /// </summary>
    public bool isReady { get; }
    /// <summary>
    /// ֲ�����������
    /// </summary>
    public int EnergyCost { get; }
    /// <summary>
    /// ֲ��ļ��ܲ���
    /// </summary>
    public void Action();
}
