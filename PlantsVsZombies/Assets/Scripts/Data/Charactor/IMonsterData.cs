using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ħ��ӿ�
/// </summary>
public interface IMonsterData:ICharactorData,IDamageReceiver
{
    /// <summary>
    /// ����ֵ
    /// </summary>
    public int Strength { get; set; }

    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    public int Speed { get; set; }
    
    /// <summary>
    /// ��ȡ��ĳ��Ԫ�صĿ���
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <returns>����ֵ��0~1��</returns>
    public float GetResistance(Elements element);
    /// <summary>
    /// ���ö�ĳ��Ԫ�صĿ���
    /// </summary>
    /// <param name="value">��ֵ</param>
    /// <param name="element">Ԫ������</param>
    public void SetResistance(float value, Elements element);

    /// <summary>
    /// ���Ԫ�ظ��ż���
    /// </summary>
    /// <param name="element">Ԫ�ظ���</param>
    /// <param name="action">�ܵ�Ԫ�ظ���ʱ���õĺ���</param>
    public void AddOnAddElementListener(Elements element, System.Action action);
    /// <summary>
    /// �Ƴ�Ԫ�ظ��ż���
    /// </summary>
    /// <param name="element">Ԫ�ظ���</param>
    /// <param name="action">�ܵ�Ԫ�ظ���ʱ���õĺ���</param>
    public void RemoveOnAddElementListener(Elements element, System.Action action);
}
