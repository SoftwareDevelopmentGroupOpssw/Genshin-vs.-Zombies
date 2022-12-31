using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// 植物控制器
/// </summary>
public class PlantsController
{
    public GameObject PlantsFatherObject = new GameObject("Plants");
    private List<Plant>[,] plants;
    public PlantsController()
    {
        ILevelData level = GameController.Instance.LevelData;
        plants = new List<Plant>[level.Col, level.Row];//x的数量是列数，y的数量是行数
        for(int i = 0; i< level.Col; i++)
        {
            for(int j = 0;j < level.Row; j++)
            {
                plants[i, j] = new List<Plant>(5);
            }
        }
    }
    /// <summary>
    /// 添加植物
    /// </summary>
    /// <param name="data">植物信息</param>
    /// <param name="gridPos">植物添加的格子位置</param>
    /// <returns>放置结果</returns>
    public bool AddPlant(ref IPlantData data,Vector2Int gridPos)
    {
        GameObject newObj = GameObject.Instantiate(data.OriginalReference,PlantsFatherObject.transform);
        Plant plant = newObj.GetComponent<Plant>();
        newObj.transform.position = GameController.Instance.GridToWorld(gridPos, GridPosition.Middle);
        
        data.GameObject = newObj;
        plant.Data = data;

        plants[gridPos.x - 1, gridPos.y - 1].Add(plant);
        return true;
    }
    /// <summary>
    /// 在指定的格子上移除最后放置的植物
    /// </summary>
    /// <param name="gridPos"></param>
    public void RemoveOnePlant(Vector2Int gridPos)
    {
        gridPos -= Vector2Int.one;
        if (plants[gridPos.x, gridPos.y].Count > 0)
        {
            int count = plants[gridPos.x, gridPos.y].Count;
            List<Plant> plantList = plants[gridPos.x, gridPos.y];
            Plant plant = plantList[count - 1];//获取最后一个
            plant.Data.GameObject = null;
            plantList.Remove(plant);
            GameObject.Destroy(plant.gameObject);
        }
    }

    /// <summary>
    /// 忽略植物规则，强制将指定的植物从地图中删除
    /// </summary>
    /// <param name="plant">删除的植物对象</param>
    public void RemovePlant(Plant plant)
    {
        Vector2Int gridPos = GameController.Instance.WorldToGrid(plant.transform.position);
        foreach(var item in plants[gridPos.x - 1,gridPos.y - 1])
        {
            if (item.Equals(plant)) 
            { 
                plant.Data.GameObject = null;
                GameObject.Destroy(plant.gameObject);
                plants[gridPos.x - 1, gridPos.y - 1].Remove(plant);
                return;
            }
        }
    }
    /// <summary>
    /// 寻找植物
    /// </summary>
    /// <param name="gridPos">寻找的格子坐标</param>
    /// <returns>所有处于格子上的植物</returns>
    public Plant[] SearchPlant(Vector2Int gridPos)
    {
        return plants[gridPos.x, gridPos.y].ToArray();
    }
    /// <summary>
    /// 寻找范围中所有的植物
    /// </summary>
    /// <param name="area">寻找的范围</param>
    /// <returns>寻找到的所有的植物</returns>
    public Plant[] SearchPlant(Area area)
    {
        List<Plant> found = new List<Plant>();
        //Vector2Int[] gridPoss = area.GetArea()
        return null;
    }
    /// <summary>
    /// 遍历所有的植物
    /// </summary>
    /// <param name="action">遍历的函数</param>
    public void Foreach(UnityAction<Plant> action)
    {

        foreach (var plantList in plants)
        {
            foreach (Plant p in plantList)
            {
                action.Invoke(p);
            }
        }
    }
    /// <summary>
    /// 清除所有的植物
    /// </summary>
    public void Clear()
    {
        foreach(var plantList in plants)
        {
            foreach (var plant in plantList)
            {
                plant.Data.GameObject = null;
                plant.Data = null;
                GameObject.Destroy(plant.gameObject);
            }
        }
        plants = null;
    }
}
