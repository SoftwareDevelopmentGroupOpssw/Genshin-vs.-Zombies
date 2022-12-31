using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 飞行物控制器
/// </summary>
public class FlyersController
{
    private static GameObject FlyersFatherObject = new GameObject("Flyers");
    /// <summary>
    /// 添加飞行物
    /// </summary>
    /// <param name="data">飞行物数据</param>
    /// <param name="worldPos">飞行物出现的世界坐标</param>
    /// <returns>飞行物对象</returns>
    public Flyer AddFlyer(IFlyerData data ,Vector3 worldPos)
    {
        //TODO:用给定的数据在像素坐标处添加一个飞行物对象
        //飞行物对象会自行销毁，因此不需要储存、查找工作
        GameObject flyer = GameObject.Instantiate(data.OriginalReference, FlyersFatherObject.transform);
        data.GameObject = flyer;
        flyer.transform.position = worldPos;
        return flyer.GetComponent<Flyer>();
    }
}
