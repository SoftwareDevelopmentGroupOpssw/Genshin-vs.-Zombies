using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �ڳ����ϴ���һ������ݻٵ�������Ϊ����Mono������������ִ��Update��Э�̡�
/// ����������Mono��Manager��
/// 1.�����ṩ���ⲿ���֡�����¼��ķ���
/// 2.�����ṩ���ⲿ��� Э�̵ķ���
/// </summary>
public class MonoManager : Singleton<MonoManager>
{
    private MonoController controller;
    [System.Obsolete("��Ӧʹ��new��ʼ��",true)]
    public MonoManager()
    {
        //��֤��MonoController�����Ψһ��
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }

    /// <summary>
    /// ���ⲿ�ṩ�� ���֡�����¼��ĺ���
    /// </summary>
    /// <param name="fun">����</param>
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }

    /// <summary>
    /// �ṩ���ⲿ �����Ƴ�֡�����¼�����
    /// </summary>
    /// <param name="fun">����</param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }
    /// <summary>
    /// �ṩ���ⲿ�������Э�̵ĺ���
    /// </summary>
    /// <param name="routine">Э�̺�������</param>
    /// <returns></returns>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }
    /// <summary>
    /// �ṩ���ⲿ�������Э�̵ĺ���
    /// </summary>
    /// <param name="methodName">Э�̺�����</param>
    /// <param name="value">Э�̺�������</param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return controller.StartCoroutine(methodName, value);
    }
    /// <summary>
    /// �ṩ���ⲿ�������Э�̵ĺ���
    /// </summary>
    /// <param name="methodName">Э�̺�����</param>
    /// <returns></returns>
    public Coroutine StartCoroutine(string methodName)
    {
        return controller.StartCoroutine(methodName);
    }
    public void StopCoroutine(Coroutine routine) => controller.StopCoroutine(routine);
    public void StopCoroutine(IEnumerator routine) => controller.StopCoroutine(routine);
}
