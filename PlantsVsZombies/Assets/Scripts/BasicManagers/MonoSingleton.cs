using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ڼ̳�MonoBehaviour��ĵ���ģʽ���ࣨ������ֻ����һ�����庬�д˽ű���
/// </summary>
/// <typeparam name="T">Ҫ��Ϊ����ģʽ����</typeparam>
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
