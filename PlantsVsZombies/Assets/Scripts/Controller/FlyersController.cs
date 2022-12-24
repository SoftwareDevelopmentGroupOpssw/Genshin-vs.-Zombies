using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 飞行物控制器
/// </summary>
public class FlyersController
{
    /// <summary>
    /// 添加飞行物
    /// </summary>
    /// <param name="data">飞行物数据</param>
    /// <param name="pixelPos">飞行物出现的像素坐标</param>
    /// <returns>飞行物对象</returns>
    public Flyer AddFlyer(IFlyerData data ,Vector2Int pixelPos)
    {
        //TODO:用给定的数据在像素坐标处添加一个飞行物对象
        //飞行物对象会自行销毁，因此不需要储存、查找工作
        throw new System.NotImplementedException();
    }
}
