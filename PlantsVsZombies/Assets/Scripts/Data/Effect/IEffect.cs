using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
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
}
