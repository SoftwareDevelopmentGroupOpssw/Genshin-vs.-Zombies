using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʹ��ԭʼԤ��������ΪKey�Ķ������
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
    /// ���������Ķ���
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
    /// �ӳ����л�ȡһ������
    /// </summary>
    /// <param name="key">ԭʼԤ����</param>
    /// <param name="action">�ڼ���֮ǰ�Ĳ���</param>
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
    /// ������з���һ������
    /// </summary>
    /// <param name="key">Ԥ��������</param>
    /// <param name="obj">��������</param>
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
/// �����
/// </summary>
/// <typeparam name="T">����������Key������</typeparam>
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
    /// ���������Ķ���
    /// </summary>
    public void Clear() => objDic.Clear();

    /// <summary>
    /// ��ѯ�Ƿ���Դ�ȡ����key���͵Ķ���
    /// �������趨��ԭʼԤ����� ���ܴ�ȡ
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsAvailable(T key) => objDic.ContainsKey(key);

    /// <summary>
    /// Ϊ��������һ���µ�ԭʼԤ������������
    /// </summary>
    /// <param name="key"></param>
    /// <param name="original"></param>
    public void SetOriginal(T key, GameObject original)
    {
        if (!objDic.ContainsKey(key))
            objDic.Add(key, new Pair() { Original = original, ObjList = new Stack<GameObject>() });
        else
        {
            if (!objDic[key].Original.Equals(original))//���������ԭʼԤ���壬��ôԭ��������Ķ����ͻᱻ���
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
    /// �ӳ����л�ȡһ������
    /// </summary>
    /// <param name="key"></param>
    /// <param name="action">�ڼ���֮ǰ�������</param>
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
