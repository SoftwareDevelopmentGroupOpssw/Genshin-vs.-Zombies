using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pvz�е���Ϸ���壨ֲ������ħ�
/// </summary>
public abstract class BaseGameobject : MonoBehaviour
{
    /// <summary>
    /// ��������
    /// </summary>
    public abstract IGameobjectData Data { get; set; }
}
