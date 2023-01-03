using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ֲ��ӿ�
/// </summary>
public interface IPlantData:ICharactorData , IDamageReceiver
{
    /// <summary>
    /// ֲ�������
    /// �ڷ���һ���µ�ֲ���ʱ�򣬻��õ��������ȥѰ�����ݿ��е���ʵ��
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
    /// ����ȴʱ�䣨���룩
    /// </summary>
    public int CoolTime { get; }
}
