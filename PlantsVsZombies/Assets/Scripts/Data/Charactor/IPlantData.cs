using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ֲ��ӿ�
/// </summary>
public interface IPlantData:ICharactorData
{
    /// <summary>
    /// ֲ�������
    /// </summary>
    public string PlantName { get; }
    /// <summary>
    /// ֲ�����������
    /// </summary>
    public int EnergyCost { get; }
    /// <summary>
    /// ����ͼƬ
    /// </summary>
    public Sprite CardSprite { get; }
    /// <summary>
    /// ����ȴʱ��
    /// </summary>
    public int CoolTime { get; }
}
