using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ԫ���˺��ӿ�
/// </summary>
public interface IElementalDamage
{
    /// <summary>
    /// �˺���С
    /// </summary>
    public int AtkDmg { get; set; }
    /// <summary>
    /// �˺�����
    /// </summary>
    public Elements ElementType { get; set; }
    /// <summary>
    /// �˴��˺��ܷ�ʩ��Ԫ��
    /// </summary>
    public bool CanAddElement { get; set; }
}
