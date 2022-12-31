using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 在场景上创建一个不会摧毁的物体作为总体Mono，可以让它来执行Update和协程。
/// 此类是总体Mono的Manager。
/// 1.可以提供给外部添加帧更新事件的方法
/// 2.可以提供给外部添加 协程的方法
/// </summary>
public class MonoManager : Singleton<MonoManager>
{
    private MonoController controller;
    [System.Obsolete("不应使用new初始化",true)]
    public MonoManager()
    {
        //保证了MonoController对象的唯一性
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }

    /// <summary>
    /// 给外部提供的 添加帧更新事件的函数
    /// </summary>
    /// <param name="fun">函数</param>
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }

    /// <summary>
    /// 提供给外部 用于移除帧更新事件函数
    /// </summary>
    /// <param name="fun">函数</param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }
    /// <summary>
    /// 提供给外部用于添加协程的函数
    /// </summary>
    /// <param name="routine">协程函数调用</param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }
    /// <summary>
    /// 提供给外部用于添加协程的函数
    /// </summary>
    /// <param name="methodName">协程函数名</param>
    /// <param name="value">协程函数参数</param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return controller.StartCoroutine(methodName, value);
    }
    /// <summary>
    /// 提供给外部用于添加协程的函数
    /// </summary>
    /// <param name="methodName">协程函数名</param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName)
    {
        return controller.StartCoroutine(methodName);
    }
    public void StopCoroutine(Coroutine routine) => controller.StopCoroutine(routine);
    public void StopCoroutine(IEnumerator routine) => controller.StopCoroutine(routine);
}
