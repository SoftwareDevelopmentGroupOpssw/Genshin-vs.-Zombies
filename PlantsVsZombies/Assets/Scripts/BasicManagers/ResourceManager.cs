using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载Manager
/// </summary>
public class ResourceManager : Singleton<ResourceManager>
{
    /// <summary>
    /// 同步加载指定类型资源，加载的资源必须在Resource文件夹下
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源在Resource文件夹的相对路径</param>
    /// <returns>资源</returns>
    public T Load<T>(string path) where T : Object
    {
        T res = Resources.Load<T>(path);
        //如果对象是一个GameObject类型的 把他实例化后 再返回出去 外部 直接使用即可
        if (res is GameObject)
            return GameObject.Instantiate(res);
        else//TextAsset AudioClip
            return res;
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源在Resource文件夹的相对路径</param>
    /// <param name="callback">资源加载完成后的处理</param>
    public void LoadAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        //开启异步加载的协程
        MonoManager.Instance.StartCoroutine(ReallyLoadAsync(path, callback));
    }

    //真正的协同程序函数  用于 开启异步加载对应的资源
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
