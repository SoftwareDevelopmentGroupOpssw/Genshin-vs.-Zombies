using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 已选择的植物
/// </summary>
public class PlantsSelected
{
    /// <summary>
    /// 植物数据
    /// </summary>
    public IPlantData Data { get; }
    /// <summary>
    /// 这个植物是否可以放置（冷却时间到了没有）
    /// </summary>
    public bool IsReady { get; }
    /// <summary>
    /// 显示在卡槽里的Sprite
    /// </summary>
    public Sprite SelectSprite { get; }
}
