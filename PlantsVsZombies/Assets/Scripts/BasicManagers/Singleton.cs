using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����ģʽ���� 
/// </summary>
/// <typeparam name="T">��Ҫ��Ϊ����ģʽ����</typeparam>
public class Singleton<T> where T: new()
{
    private static T instance;
    /// <summary>
    /// ʵ��
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
