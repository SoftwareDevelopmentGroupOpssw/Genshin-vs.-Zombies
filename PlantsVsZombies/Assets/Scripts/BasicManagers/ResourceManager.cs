using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��Դ����Manager
/// </summary>
public class ResourceManager : Singleton<ResourceManager>
{
    /// <summary>
    /// ͬ������ָ��������Դ�����ص���Դ������Resource�ļ�����
    /// </summary>
    /// <typeparam name="T">��Դ����</typeparam>
    /// <param name="path">��Դ��Resource�ļ��е����·��</param>
    /// <returns>��Դ</returns>
    public T Load<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);
        //���������һ��GameObject���͵� ����ʵ������ �ٷ��س�ȥ �ⲿ ֱ��ʹ�ü���
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else//TextAsset AudioClip
            return res;
    }

    /// <summary>
    /// �첽������Դ
    /// </summary>
    /// <typeparam name="T">��Դ����</typeparam>
    /// <param name="path">��Դ��Resource�ļ��е����·��</param>
    /// <param name="callback">��Դ������ɺ�Ĵ���</param>
    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        //�����첽���ص�Э��
        MonoManager.Instance.StartCoroutine(ReallyLoadAsync(path, callback));
    }

    //������Эͬ������  ���� �����첽���ض�Ӧ����Դ
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if (r.asset is GameObject)
            callback(GameObject.Instantiate(r.asset) as T);
        else
            callback(r.asset as T);
    }

}
