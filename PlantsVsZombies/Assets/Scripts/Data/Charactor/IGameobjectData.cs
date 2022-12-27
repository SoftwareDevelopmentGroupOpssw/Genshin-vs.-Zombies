using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ϸ����ӿ�
/// </summary>
public interface IGameobjectData
{
    /// <summary>
    /// ��˶����������Ч��
    /// </summary>
    /// <param name="effect">Ч��</param>
    public void AddEffect(IEffect effect);
    /// <summary>
    /// ��˶��������Ƴ�Ч��
    /// </summary>
    /// <param name="effect">Ч��</param>
    public void RemoveEffect(IEffect effect);
    /// <summary>
    /// ��ȡ/���� ��ǰdata�ڳ�������������Ϸ�������
    /// </summary>
    public GameObject GameObject { get; set; }
    /// <summary>
    /// ��Ϸ�����ԭʼԤ���壬ʹ��������ʵ��������
    /// </summary>
    public GameObject OriginalReference { get; }
    /// <summary>
    /// ����Ϸ�������ʱ����
    /// </summary>
    public void OnAwake();
    /// <summary>
    /// ����Ϸ��������ʱ����
    /// </summary>
    public void OnDestroy();
}
