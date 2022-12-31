using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ч��״̬
/// </summary>
public enum EffectState
{
    /// <summary>
    /// Ч���ձ���ӣ���δ����
    /// </summary>
    Initialized,
    /// <summary>
    /// Ч���Ѿ�����ӣ����ڴ���
    /// </summary>
    Processing,
    /// <summary>
    /// Ч���������ȴ���ɾ��
    /// </summary>
    End,
    /// <summary>
    /// Ч����ִ���г���
    /// </summary>
    Error,
}
/// <summary>
/// Ч���ӿ�
/// </summary>
public interface IEffect
{
    /// <summary>
    /// Ч����
    /// ��ͬ���͵�Ч�������綼���ƶ��ٶ�˥��Ч����������ͬ
    /// </summary>
    public string EffectName { get; }
    /// <summary>
    /// ���Ч��������״̬
    /// </summary>
    public EffectState State { get; }
    /// <summary>
    /// ʩ����
    /// </summary>
    public IGameobjectData Caster { get; }

    /// <summary>
    /// ִ��Ч��
    /// </summary>
    /// <param name="target">Ч������</param>
    public void EnableEffect(IGameobjectData target);
    /// <summary>
    /// �Ƴ�Ч��
    /// </summary>
    /// <param name="target">Ч������</param>
    public void DisableEffect(IGameobjectData target);
    /// <summary>
    /// ֡���µ��ú���
    /// </summary>
    /// <param name="target">����</param>
    public void UpdateEffect(IGameobjectData target);
}
