using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ϸ����ӿ�
/// </summary>
public interface IGameobjectData
{
    /// <summary>
    /// ��ȡ/���� ��ǰdata�ڳ�������������Ϸ�������
    /// </summary>
    public GameObject GameObject { get; set; }
    /// <summary>
    /// ��Ϸ�������Դ·�����Ա�ʹ��Resouces.Load��������
    /// </summary>
    public string ResourcePath { get; }
    /// <summary>
    /// ����Ϸ�������ʱ����
    /// </summary>
    public void OnAwake();
    /// <summary>
    /// ����Ϸ��������ʱ����
    /// </summary>
    public void OnDestroy();
    /// <summary>
    /// ����һ���뵱ǰ������ͬ���ͣ���������ΪĬ��ֵ��Data
    /// </summary>
    public IGameobjectData Instantiate();
}
