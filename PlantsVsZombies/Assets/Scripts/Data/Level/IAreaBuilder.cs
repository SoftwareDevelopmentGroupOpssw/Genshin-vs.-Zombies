using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBuilder:Area
{
    /// <summary>
    /// 增加一个Area
    /// </summary>
    /// <param name="area">Area对象</param>
    public void AddArea(Area area)
    {
        //TODO:添加一个Area到Builder中去
    }
    /// <summary>
    /// 获取这个builder下所有area格点的并集
    /// </summary>
    /// <param name="level">关卡数据</param>
    /// <param name="gridPos">格点坐标</param>
    /// <returns>被纳入范围的格点集合</returns>
    public override Vector2Int[] GetArea(ILevelData level, Vector2Int gridPos)
    {
        //TODO：将所有的Area给出的格点取并集，并返回出去
        throw new System.NotImplementedException();
    }
}
