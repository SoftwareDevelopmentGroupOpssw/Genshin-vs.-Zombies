using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 植物控制器
/// </summary>
public class PlantsController
{
    private GameObject PlantsFatherObject = new GameObject("Plants");
    private ILevelData levelData;
    private Plant[,] plants;
    public PlantsController(ILevelData level)
    {
        this.levelData = level;
        plants = new Plant[levelData.Row, levelData.Col];
    }
    /// <summary>
    /// 强制添加植物
    /// </summary>
    /// <param name="data">植物信息</param>
    /// <param name="gridPos">植物添加的格子位置</param>
    /// <returns>植物对象</returns>
    public Plant AddPlant(IPlantData data,Vector2Int gridPos)
    {
        GameObject newObj = GameObject.Instantiate(data.OriginalReference,PlantsFatherObject.transform);
        Plant plant = newObj.GetComponent<Plant>();
        newObj.transform.position = levelData.GridToWorld(gridPos, GridPosition.Middle, GameController.Instance.Level.transform.position);
        data.GameObject = newObj;
        return plant;
    }
    public bool TryAddPlant(IPlantData data,Vector2Int gridPos, out Plant plant)
    {
        GameObject newObj = GameObject.Instantiate(data.OriginalReference,PlantsFatherObject.transform);
        plant = newObj.GetComponent<Plant>();
        newObj.transform.position = levelData.GridToWorld(gridPos, GridPosition.Middle, GameController.Instance.Level.transform.position);
        data.GameObject = newObj;
        return true;
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
