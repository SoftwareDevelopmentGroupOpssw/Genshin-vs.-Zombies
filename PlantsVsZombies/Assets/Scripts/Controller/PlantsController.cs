using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 植物控制器
/// </summary>
public class PlantsController
{
    private ILevelData levelData;
    public PlantsController(ILevelData level)
    {
        this.levelData = level;
    }
    /// <summary>
    /// 添加植物
    /// </summary>
    /// <param name="data">植物信息</param>
    /// <param name="gridPos">植物添加的格子位置</param>
    /// <returns>植物对象</returns>
    public Plant AddPlant(IPlantData data,Vector2Int gridPos)
    {
        //TODO:用给定的数据在格子坐标处添加一个植物对象， 并储存起来
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 将植物从地图中删除
    /// </summary>
    /// <param name="plant">删除的植物对象</param>
    public void RemovePlant(Plant plant)
    {
        //TODO:删除植物对象
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 寻找植物
    /// </summary>
    /// <param name="gridPos">寻找的格子坐标</param>
    /// <returns>所有处于格子上的植物</returns>
    public Plant[] SearchPlant(Vector2Int gridPos)
    {
        //TODO:在指定格子坐标上寻找所有的植物
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 寻找范围中所有的植物
    /// </summary>
    /// <param name="area">寻找的范围</param>
    /// <returns>寻找到的所有的植物</returns>
    public Plant[] SearchPlant(Area area)
    {
        //TODO:在指定范围中上寻找所有的植物
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 遍历所有的植物
    /// </summary>
    /// <param name="action">遍历的函数</param>
    public void Foreach(UnityAction<Plant> action)
    {
        //TODO：遍历其中
    }
}
