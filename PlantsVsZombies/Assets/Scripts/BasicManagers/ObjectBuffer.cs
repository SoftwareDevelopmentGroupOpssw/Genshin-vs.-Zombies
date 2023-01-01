using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使用原始预制体来作为Key的对象池子
/// </summary>
public class ObjectBuffer
{

    private Transform fatherTransform;
    private Dictionary<GameObject, Stack<GameObject>> objDic = new Dictionary<GameObject, Stack<GameObject>>();


    public ObjectBuffer(Transform parent)
    {
        fatherTransform = parent;
    }
    /// <summary>
    /// 清除池子里的东西
    /// </summary>
    public void Clear()
    {
        foreach(var item in objDic)
        {
            foreach(var obj in item.Value)
            {
                GameObject.Destroy(obj);
            }
        }
        objDic.Clear();
    }

    /// <summary>
    /// 从池子中获取一个物体
    /// </summary>
    /// <param name="key">原始预制体</param>
    /// <param name="action">在激活之前的操作</param>
    /// <returns></returns>
    public GameObject Get(GameObject key, System.Action<GameObject> action = null)
    {
        if (!objDic.ContainsKey(key))
            objDic.Add(key, new Stack<GameObject>());
        
        Stack<GameObject> objList = objDic[key];
        if (objList.Count > 0)
        {
            GameObject obj = objList.Pop();
            action?.Invoke(obj);
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = GameObject.Instantiate(key, fatherTransform);
            action?.Invoke(obj);
            return obj;
        }
    }
    /// <summary>
    /// 向池子中放入一个物体
    /// </summary>
    /// <param name="key">预制体类型</param>
    /// <param name="obj">对象物体</param>
    public void Put(GameObject key, GameObject obj)
    {
        if (!objDic[key].Contains(obj))
        {
            objDic[key].Push(obj);
            obj.SetActive(false);
        }
    }
}

/// <summary>
/// 物体池
/// </summary>
/// <typeparam name="T">用来搜索的Key的类型</typeparam>
public class ObjectBuffer<T>
{
    class Pair
    {
        public GameObject Original;
        public Stack<GameObject> ObjList;
    }
    private Transform fatherTransform;
    private Dictionary<T, Pair> objDic = new Dictionary<T, Pair>();


    public ObjectBuffer(Transform parent)
    {
        fatherTransform = parent;
    }

    /// <summary>
    /// 清除池子里的东西
    /// </summary>
    public void Clear() => objDic.Clear();

    /// <summary>
    /// 查询是否可以存取此种key类型的对象
    /// 必须先设定过原始预制体后 才能存取
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsAvailable(T key) => objDic.ContainsKey(key);

    /// <summary>
    /// 为池子设置一个新的原始预制体用来复制
    /// </summary>
    /// <param name="key"></param>
    /// <param name="original"></param>
    public void SetOriginal(T key, GameObject original)
    {
        if (!objDic.ContainsKey(key))
            objDic.Add(key, new Pair() { Original = original, ObjList = new Stack<GameObject>() });
        else
        {
            if (!objDic[key].Original.Equals(original))//如果更改了原始预制体，那么原来池子里的东西就会被清空
            {
                foreach (var obj in objDic[key].ObjList)
                {
                    GameObject.Destroy(obj);
                }
                objDic[key].ObjList.Clear();
            }
            objDic[key].Original = original;
        }
    }
    /// <summary>
    /// 从池子中获取一个物体
    /// </summary>
    /// <param name="key"></param>
    /// <param name="action">在激活之前对其操作</param>
    /// <returns></returns>
    public GameObject Get(T key, System.Action<GameObject> action = null)
    {
        if (!objDic.ContainsKey(key))
            throw new System.InvalidOperationException("The original of gameobject has not set yet.");
        else
        {
            Pair pair = objDic[key];
            GameObject obj;
            if(pair.ObjList.Count > 0)
            {
                obj = pair.ObjList.Pop();
            }
            else
            {
                obj = GameObject.Instantiate(pair.Original, fatherTransform);
            }
            action?.Invoke(obj);
            return obj;
        }
    }
    public void Put(T key, GameObject obj)
    {
        if (!objDic.ContainsKey(key))
            throw new System.InvalidOperationException("The original of gameobject has not set yet.");
        else
        {
            objDic[key].ObjList.Push(obj);
        }
    }
}
