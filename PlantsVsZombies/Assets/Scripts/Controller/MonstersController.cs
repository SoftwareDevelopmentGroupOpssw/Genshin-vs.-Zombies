using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 魔物控制器
/// </summary>
public class MonstersController
{
    private ILevelData levelData;
    public MonstersController(ILevelData level)
    {
        this.levelData = level;
    }
    /// <summary>
    /// 在指定的像素位置添加魔物
    /// </summary>
    /// <param name="data">魔物数据</param>
    /// <param name="pixelPos">像素位置</param>
    /// <returns>魔物对象</returns>
    public Monster AddMonster(IMonsterData data, Vector2Int pixelPos)
    {
        //TODO:添加魔物并储存
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 移除魔物
    /// </summary>
    /// <param name="monster">魔物对象</param>
    public void RemoveMonster(Monster monster)
    {
        //TODO:移除魔物
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 查询指定格子坐标中最前方的魔物
    /// </summary>
    /// <param name="gridPos">格子坐标</param>
    /// <returns>最前方的魔物对象</returns>
    public Monster SearchFirstMonster(Vector2Int gridPos)
    {
        //TODO:查询单个魔物
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 查询指定格子中所有的魔物
    /// </summary>
    /// <param name="gridPos">格子对象</param>
    /// <returns>魔物对象数组</returns>
    public Monster[] SearchMonsters(Vector2Int gridPos)
    {
        //TODO:查询魔物
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 搜索指定范围中的魔物并排序
    /// 排序优先级为：第一优先级前方到后方，第二优先级上方到下方
    /// </summary>
    /// <param name="area"></param>
    /// <returns>指定范围中的魔物的有序序列</returns>
    public Monster[] SearchMonsters(Area area)
    {
        //TODO:查询指定区域中的魔物并返回有序集合
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 遍历所有的魔物
    /// </summary>
    /// <param name="action">遍历的函数</param>
    public void Foreach(UnityAction<Monster> action)
    {
        //TODO：遍历其中
    }
}
