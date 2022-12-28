using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 将格子坐标转化为像素坐标时，使用的参考点位置
/// </summary>
public enum GridPosition
{
    /// <summary>
    /// 格子左边中间的像素位置
    /// </summary>
    Left,
    /// <summary>
    /// 格子正中心的像素位置
    /// </summary>
    Middle,
    /// <summary>
    /// 格子右边中间的像素位置
    /// </summary>
    Right,
}
public interface ILevelData
{
    /// <summary>
    /// 行数
    /// </summary>
    public int Row { get; }
    /// <summary>
    /// 列数
    /// </summary>
    public int Col { get; }
    /// <summary>
    /// 地图的Sprite
    /// </summary>
    public Sprite Sprite { get; }
    /// <summary>
    /// 地图的出怪序列
    /// </summary>
    public Queue<IMonsterData> MonsterList { get; }
    /// <summary>
    /// 将一个世界坐标转换为地图的格子坐标
    /// 格子坐标以左上角为原点，向右为x轴，向下为y轴
    /// </summary>
    /// <param name="worldPos">世界坐标</param>
    /// <param name="levelPos">现在关卡背景所处的位置</param>
    /// <returns>格子坐标，如果在格子之外则返回(-1,-1)</returns>
    public Vector2Int WorldToGrid(Vector3 worldPos, Vector3 levelPos);
    /// <summary>
    /// 将一个格子坐标转换为世界坐标
    /// 格子坐标以左上角为原点，向右为x轴，向下为y轴
    /// </summary>
    /// <param name="gridPos">格子坐标</param>
    /// <param name="pos">转化为世界坐标时的偏移枚举</param>
    /// <param name="levelPos">现在关卡背景所处的位置</param>
    /// <returns>对应偏移处的世界坐标</returns>
    public Vector3 GridToWorld(Vector2Int gridPos, GridPosition pos,Vector3 levelPos);
}
