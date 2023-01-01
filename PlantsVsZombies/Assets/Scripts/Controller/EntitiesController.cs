using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ʵ�������
/// �ṩ��һ����GameController��ͣ�Ľӿ�
/// �ó�����ʹ��ObjectBufferƵ�����ֵĶ���õ�����
/// </summary>
public class EntitiesController
{
    /// <summary>
    /// ������ֵ�����λ������
    /// </summary>
    public class ObjectUnite
    {
        /// <summary>
        /// �����ڳ�����ĸ�����
        /// </summary>
        public ObjectBuffer Buffer { get; set; }
        /// <summary>
        /// ���ڼ���״̬�е�����
        /// </summary>
        public Dictionary<GameObject, List<GameObject>> ActiveLists { get; } = new Dictionary<GameObject, List<GameObject>>();
        /// <summary>
        /// ���������Ϸ����ű�
        /// </summary>
        public Dictionary<GameObject, List<System.Type>> ManagedBehaviours { get; } = new Dictionary<GameObject, List<System.Type>>();
    }
    private Dictionary<string, ObjectUnite> bufferDic = new Dictionary<string, ObjectUnite>();

    /// <summary>
    /// �����ֵĳ����Ƿ񱻹���
    /// </summary>
    /// <param name="bufferName"></param>
    /// <returns></returns>
    public bool ContainsBuffer(string bufferName) => bufferDic.ContainsKey(bufferName);

    /// <summary>
    /// ������Behaviour�ű��������
    /// </summary>
    /// <typeparam name="T">Behaviour�ű�������</typeparam>
    /// <param name="bufferName"></param>
    /// <param name="key"></param>
    public void AddBehaviourManagement<T>(string bufferName, GameObject key)where T : Behaviour
    {
        if (bufferDic.ContainsKey(bufferName))
        {
            var dic = bufferDic[bufferName].ManagedBehaviours;
            if (!dic.ContainsKey(key))
                dic[key] = new List<System.Type>();
            dic[key].Add(typeof(T));
        }
    }
    /// <summary>
    /// ������Behaviour�ű��ӹ������Ƴ�
    /// </summary>
    /// <typeparam name="T">Behaviour�ű�������</typeparam>
    /// <param name="bufferName"></param>
    /// <param name="key"></param>
    public void RemoveBehaviourManagement<T>(string bufferName,GameObject key) where T:Behaviour
    {
        if (bufferDic.ContainsKey(bufferName))
        {
            var dic = bufferDic[bufferName].ManagedBehaviours;
            if (dic.ContainsKey(key))
                dic[key].Remove(typeof(T));
        }
    }
    /// <summary>
    /// ���һ�����ӵ����������������е���������õ�����
    /// Get��Put�������ɵĶ�����ᱻ����
    /// </summary>
    /// <param name="bufferName"></param>
    /// <param name="buffer"></param>
    public void AddBufferToManagement(string bufferName, ObjectBuffer buffer) 
    {
        if (buffer != null)
            bufferDic.Add(bufferName, new ObjectUnite() { Buffer = buffer });
    }

    /// <summary>
    /// �ӹ��������Ƴ�һ��������ĳ���
    /// Get��Put�������ɵĶ�����ᱻ����
    /// </summary>
    /// <param name="bufferName"></param>
    /// <returns>����أ�������Ҳ����ͷ���null</returns>
    public ObjectBuffer RemoveBufferFromManagement(string bufferName)
    {
        if (bufferDic.ContainsKey(bufferName))
        {
            ObjectBuffer buffer = bufferDic[bufferName].Buffer;
            bufferDic.Remove(bufferName);
            return buffer;
        }
        else
            return null;
    }
    /// <summary>
    /// �ø��������ֲ��ұ���������ϣ��������������е�Key�л�ȡһ����Ϸ����
    /// ��ȡ�Ķ���ᱻ�Զ�����List���й���
    /// </summary>
    /// <param name="bufferName"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public GameObject Get(string bufferName, GameObject key)
    {
        if (bufferDic.ContainsKey(bufferName))
        {
            GameObject instance = bufferDic[bufferName].Buffer.Get(key);
            if (!bufferDic[bufferName].ActiveLists.ContainsKey(key))
                bufferDic[bufferName].ActiveLists.Add(key, new List<GameObject>());
            bufferDic[bufferName].ActiveLists[key].Add(instance);
            return instance;
        }
        else
            return null;
    }
    /// <summary>
    /// �ø��������ֲ��ұ���������ϣ�����һ����Ϸ���󵽶���ص�Key��
    /// ����Ķ�����List�б��Ƴ����й���
    /// </summary>
    /// <param name="bufferName"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public void Put(string bufferName,GameObject key,GameObject instance)
    {
        if (bufferDic.ContainsKey(bufferName))
        {
            bufferDic[bufferName].Buffer.Put(key,instance);
            
            if (bufferDic[bufferName].ActiveLists.ContainsKey(key))
                bufferDic[bufferName].ActiveLists[key].Remove(instance);
        }
    }
    /// <summary>
    /// ����
    /// ֻ��Ŀǰ����Ķ������
    /// </summary>
    /// <param name="action"></param>
    public void Foreach(System.Action<ObjectUnite> action)
    {
        foreach(var unite in bufferDic)
        {
           action.Invoke(unite.Value);
        }
    }
    /// <summary>
    /// ���ĳһ��������е����ж���
    /// </summary>
    /// <param name="bufferName"></param>
    public void ClearBuffer(string bufferName)
    {
        if (bufferDic.ContainsKey(bufferName))
        {
            bufferDic[bufferName].Buffer.Clear();
        }
    }
    /// <summary>
    /// ������ж�����еĶ������ã�
    /// �����Ѿ�����Ķ���
    /// </summary>
    public void ClearAll()
    {
        foreach(var unite in bufferDic.Values)
        {
            unite.Buffer.Clear();
            unite.ManagedBehaviours.Clear();
            foreach(var objList in unite.ActiveLists.Values)
            {
                objList.ForEach((obj) => GameObject.Destroy(obj));
            }
        }
    }

    public ObjectBuffer this[string bufferName]
    {
        get
        {
            return bufferDic[bufferName].Buffer;
        }
    }
}
