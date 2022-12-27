using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ԫ���йص��¼�
/// </summary>
public class ElementEvent
{
    private readonly static int ELEMENTS_COUNT = System.Enum.GetValues(typeof(Elements)).Length;
    
    private System.Action[] actions = new System.Action[ELEMENTS_COUNT];

    /// <summary>
    /// ���Ԫ�ؼ���
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <param name="action">����</param>
    public void AddListener(Elements element, System.Action action) => actions[(int)element] += action;
    /// <summary>
    /// �Ƴ�Ԫ�ؼ���
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <param name="action">����</param>
    public void RemoveListener(Elements element, System.Action action) => actions[(int)element] -= action;
    /// <summary>
    /// ����Ԫ�ؼ���
    /// </summary>
    /// <param name="element">Ԫ������</param>
    public void Trigger(Elements element) => actions[(int)element]?.Invoke();
}
/// <summary>
/// ��Ԫ���йص��¼�
/// </summary>
/// <typeparam name="T">�¼������Ĳ�������</typeparam>
public class ElementEvent<T>
{
    public const int ELEMENTS_COUNT = 8;
    private System.Action<T>[] actions = new System.Action<T>[ELEMENTS_COUNT];

    /// <summary>
    /// ���Ԫ�ؼ���
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <param name="action">����</param>
    public void AddListener(Elements element, System.Action<T> action) => actions[(int)element] += action;
    /// <summary>
    /// �Ƴ�Ԫ�ؼ���
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <param name="action">����</param>
    public void RemoveListener(Elements element, System.Action<T> action) => actions[(int)element] -= action;
    /// <summary>
    /// ����Ԫ�ؼ���
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <param name="item">��������</param>
    public void Trigger(Elements element, T item) => actions[(int)element]?.Invoke(item);
}
