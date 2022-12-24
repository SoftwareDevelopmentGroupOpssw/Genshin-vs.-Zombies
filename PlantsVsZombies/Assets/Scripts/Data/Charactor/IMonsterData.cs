using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ħ��ӿ�
/// </summary>
public interface IMonsterData:ICharactorData
{
    /// <summary>
    /// ����ֵ
    /// </summary>
    public int Strength { get; set; }
    /// <summary>
    /// ��ȡ��ĳ��Ԫ�صĿ���
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <returns>����ֵ��0~1��</returns>
    public float GetResistance(Elements element);
    /// <summary>
    /// ��Ŀ�괦�ܵ��˺�
    /// </summary>
    /// <param name="causer">�ܵ��˺�����Դ</param>
    public void ReceiveDamage(IElementalDamage causer);
}
