using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ա��˺�������
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// ��ȡһ���˺��������������˺�
    /// </summary>
    /// <returns>�˺�������</returns>
    public IDamageReceiver GetReceiver();
}
