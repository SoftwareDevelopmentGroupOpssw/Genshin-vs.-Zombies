using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ħ��״̬����
/// </summary>
public class MonsterState
{
    protected MonsterState() { }
    /// <summary>
    /// ����״̬ʱ����
    /// </summary>
    public virtual void OnEnterState() { }
    /// <summary>
    /// �ڴ�״̬ʱ֡���º���
    /// </summary>
    public virtual void Update() { }
    /// <summary>
    /// �˳�״̬ʱ����
    /// </summary>
    public virtual void OnExitState() { }
}
