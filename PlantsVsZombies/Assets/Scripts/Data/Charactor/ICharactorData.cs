using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ɫ�ӿ�
/// </summary>
public interface ICharactorData:IGameobjectData
{
    /// <summary>
    /// ��ɫ��ǰ����ֵ
    /// </summary>
    public int Health { get;}
    /// <summary>
    /// ��ɫ������
    /// </summary>
    public int AtkPower { get; }
    /// <summary>
    /// ֡����ʱ���õĲ���
    /// </summary>
    public void Action();
}
