using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 对于继承MonoBehaviour类的单例模式基类（场景上只存在一个物体含有此脚本）
/// </summary>
/// <typeparam name="T">要成为单例模式的类</typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get => instance;
    }
    void Awake()
    {
        instance = this as T;
    }
}
