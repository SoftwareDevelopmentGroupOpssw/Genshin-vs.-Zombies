using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 地图范围基类
/// </summary>
public abstract class Area
{
    /// <summary>
    /// 获取这个地图上，以指定格子坐标，所有的可行格子坐标范围
    /// </summary>
    /// <param name="level">关卡数据</param>
    /// <param name="gridPos">格子坐标</param>
    /// <returns>所有被纳入范围的格子坐标</returns>
    public abstract Vector2Int[] GetArea(ILevelData level, Vector2Int gridPos);
}
