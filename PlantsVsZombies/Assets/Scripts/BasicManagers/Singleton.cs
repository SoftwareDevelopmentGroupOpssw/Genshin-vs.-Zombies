using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 单例模式基类 
/// </summary>
/// <typeparam name="T">需要成为单例模式的类</typeparam>
public class Singleton<T> where T: new()
{
    private static T instance;
    /// <summary>
    /// 实例
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
}
