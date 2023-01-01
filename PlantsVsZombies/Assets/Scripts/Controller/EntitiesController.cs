using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 实体控制器
/// 提供了一个向GameController暂停的接口
/// 让场景上使用ObjectBuffer频繁出现的对象得到管理
/// </summary>
public class EntitiesController
{
    /// <summary>
    /// 物体出现的所有位置联合
    /// </summary>
    public class ObjectUnite
    {
        /// <summary>
        /// 出现在池子里的复制体
        /// </summary>
        public ObjectBuffer Buffer { get; set; }
        /// <summary>
        /// 正在激活状态中的物体
        /// </summary>
        public Dictionary<GameObject, List<GameObject>> ActiveLists { get; } = new Dictionary<GameObject, List<GameObject>>();
        /// <summary>
        /// 被管理的游戏物体脚本
        /// </summary>
        public Dictionary<GameObject, List<System.Type>> ManagedBehaviours { get; } = new Dictionary<GameObject, List<System.Type>>();
    }
    private Dictionary<string, ObjectUnite> bufferDic = new Dictionary<string, ObjectUnite>();

    /// <summary>
    /// 此名字的池子是否被管理
    /// </summary>
    /// <param name="bufferName"></param>
    /// <returns></returns>
    public bool ContainsBuffer(string bufferName) => bufferDic.ContainsKey(bufferName);

    /// <summary>
    /// 将此种Behaviour脚本纳入管理
    /// </summary>
    /// <typeparam name="T">Behaviour脚本的类型</typeparam>
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
    /// 将此种Behaviour脚本从管理中移除
    /// </summary>
    /// <typeparam name="T">Behaviour脚本的类型</typeparam>
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
    /// 添加一个池子到管理器中来让其中的所有物体得到管理
    /// Get和Put操作生成的对象均会被管理
    /// </summary>
    /// <param name="bufferName"></param>
    /// <param name="buffer"></param>
    public void AddBufferToManagement(string bufferName, ObjectBuffer buffer) 
    {
        if (buffer != null)
            bufferDic.Add(bufferName, new ObjectUnite() { Buffer = buffer });
    }

    /// <summary>
    /// 从管理器中移除一个被管理的池子
    /// Get和Put操作生成的对象均会被管理
    /// </summary>
    /// <param name="bufferName"></param>
    /// <returns>对象池，如果查找不到就返回null</returns>
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
    /// 用给定的名字查找被管理的联合，从联合里对象池中的Key中获取一个游戏对象
    /// 获取的对象会被自动加入List进行管理
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
    /// 用给定的名字查找被管理的联合，放入一个游戏对象到对象池的Key中
    /// 放入的对象会从List中被移除进行管理
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
    /// 遍历
    /// 只对目前激活的对象遍历
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
    /// 清除某一个对象池中的所有对象
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
    /// 清除所有对象池中的对象（慎用）
    /// 包括已经激活的对象
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
