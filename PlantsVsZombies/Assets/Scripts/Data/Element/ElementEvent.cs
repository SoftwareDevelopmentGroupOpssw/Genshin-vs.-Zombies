using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 和元素有关的事件
/// </summary>
public class ElementEvent
{
    private readonly static int ELEMENTS_COUNT = System.Enum.GetValues(typeof(Elements)).Length;
    
    private System.Action[] actions = new System.Action[ELEMENTS_COUNT];

    /// <summary>
    /// 添加元素监听
    /// </summary>
    /// <param name="element">元素类型</param>
    /// <param name="action">操作</param>
    public void AddListener(Elements element, System.Action action) => actions[(int)element] += action;
    /// <summary>
    /// 移除元素监听
    /// </summary>
    /// <param name="element">元素类型</param>
    /// <param name="action">操作</param>
    public void RemoveListener(Elements element, System.Action action) => actions[(int)element] -= action;
    /// <summary>
    /// 触发元素监听
    /// </summary>
    /// <param name="element">元素类型</param>
    public void Trigger(Elements element) => actions[(int)element]?.Invoke();
}
/// <summary>
/// 和元素有关的事件
/// </summary>
/// <typeparam name="T">事件函数的参数类型</typeparam>
public class ElementEvent<T>
{
    public const int ELEMENTS_COUNT = 8;
    private System.Action<T>[] actions = new System.Action<T>[ELEMENTS_COUNT];

    /// <summary>
    /// 添加元素监听
    /// </summary>
    /// <param name="element">元素类型</param>
    /// <param name="action">操作</param>
    public void AddListener(Elements element, System.Action<T> action) => actions[(int)element] += action;
    /// <summary>
    /// 移除元素监听
    /// </summary>
    /// <param name="element">元素类型</param>
    /// <param name="action">操作</param>
    public void RemoveListener(Elements element, System.Action<T> action) => actions[(int)element] -= action;
    /// <summary>
    /// 触发元素监听
    /// </summary>
    /// <param name="element">元素类型</param>
    /// <param name="item">触发物体</param>
    public void Trigger(Elements element, T item) => actions[(int)element]?.Invoke(item);
}
