using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 从指定位置到地图最右端一行的范围
/// </summary>
public class FrontLine : Area
{
    public override Vector2Int[] GetArea(ILevelData level, Vector2Int gridPos)
    {
        if (gridPos.x > level.Col || gridPos.y > level.Row)
            return new Vector2Int[0];
        else
        {
            List<Vector2Int> posList = new List<Vector2Int>();
            for(int i = gridPos.x; i <= level.Col; i++)
            {
                posList.Add(new Vector2Int(i, gridPos.y));//寻找一行的位置
            }
            return posList.ToArray();
        }
    }
}
