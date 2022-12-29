using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ֲ��ű�����
/// </summary>
public abstract class Plant : BaseGameobject
{
    /// <summary>
    /// ֲ��������Ϣ
    /// </summary>
    public IPlantData Data { get; set; }
    /// <summary>
    /// ����ʱ���õ��¼�
    /// </summary>
    public event UnityAction<Plant> OnDie;
    protected virtual void OnDestroy()
    {
        OnDie?.Invoke(this);
    }
}
