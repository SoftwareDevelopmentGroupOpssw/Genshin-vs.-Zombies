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
    public int Health { get; set; }
    /// <summary>
    /// ��ɫ������
    /// </summary>
    public int AtkPower { get; set; }
}
