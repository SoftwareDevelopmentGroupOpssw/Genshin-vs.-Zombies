using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 飞行物控制器
/// </summary>
public class FlyersController
{
    public readonly static GameObject FlyersFatherObject = new GameObject("Flyers");
    /// <summary>
    /// 对象池
    /// 用原始预制体作为Key，生成的实例列表为Value
    /// 如果不够用了再用预制体生成
    /// </summary>
    private ObjectBuffer flyerPool = new ObjectBuffer(FlyersFatherObject.transform);

    /// <summary>
    /// 添加飞行物
    /// 会给飞行物数据和飞行物脚本添加双向链表（IFlyerData.Gameobject、Flyer.Data）
    /// </summary>
    /// <param name="data">飞行物数据</param>
    /// <param name="worldPos">飞行物出现的世界坐标</param>
    /// <param name="callBack">飞行物在激活前调用的函数</param>
    /// <returns>飞行物对象的脚本</returns>
    public void AddFlyer(IFlyerData data, Vector3 worldPos, UnityAction<Flyer> callback) => AddFlyer<Flyer>(data,worldPos, callback);
    /// <summary>
    /// 添加飞行物对象
    /// </summary>
    /// <typeparam name="T">转换的脚本类型</typeparam>
    /// <param name="data">飞行物数据</param>
    /// <param name="worldPos">飞行物出现的世界坐标</param>
    /// <param name="callBack">飞行物在激活前调用的函数</param>
    /// <returns>飞行物对象脚本</returns>
    public void AddFlyer<T>(IFlyerData data, Vector3 worldPos, UnityAction<T> callBack = null) where T : Flyer 
    {
        GameObject obj = flyerPool.Get(data.OriginalReference);
        obj.transform.position = worldPos;
        //设置双向链表
        T flyer = obj.GetComponent<T>();//获取身上的飞行物脚本
        flyer.Data = data;
        data.GameObject = obj;

        callBack?.Invoke(flyer);//调用函数
    }
    /// <summary>
    /// 将飞行物从场景上移除
    /// 取消飞行物数据和飞行物脚本之间的双向链表
    /// </summary>
    /// <param name="flyer">要移除的飞行物</param>
    public void RemoveFlyer(Flyer flyer)
    {
        flyerPool.Put(flyer.Data.OriginalReference, flyer.gameObject);

        //取消关联链表
        flyer.Data.GameObject = null;
        flyer.Data = null;

    }
}
