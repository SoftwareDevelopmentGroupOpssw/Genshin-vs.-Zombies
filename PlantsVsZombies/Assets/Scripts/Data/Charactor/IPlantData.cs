using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 植物接口
/// </summary>
public interface IPlantData:ICharactorData
{
    /// <summary>
    /// 植物是否准备好下次行动
    /// </summary>
    public bool isReady { get; }
    /// <summary>
    /// 植物的能量花费
    /// </summary>
    public int EnergyCost { get; }
    /// <summary>
    /// 卡槽图片
    /// </summary>
    public Sprite CardSprite { get; }
}
