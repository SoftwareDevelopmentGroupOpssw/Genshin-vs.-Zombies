using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ɹ���Ĳ��Ժ���
/// </summary>
public interface IGenerateStrategy
{
    /// <summary>
    /// ���ɵĵ�����
    /// ÿ�ε����ᴦ����Ӧ�߼�
    /// </summary>
    /// <returns></returns>
    public IEnumerator GenerateEnumerator();
}
