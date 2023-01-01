using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 植物接口
/// </summary>
public interface IPlantData:ICharactorData , IDamageReceiver
{
    /// <summary>
    /// 植物的名字
    /// </summary>
    public string PlantName { get; }
    /// <summary>
    /// 植物的能量花费
    /// </summary>
    public int EnergyCost { get; }
    /// <summary>
    /// 卡槽图片
    /// </summary>
    public Sprite CardSprite { get; }
    /// <summary>
    /// 总冷却时间（毫秒）
    /// </summary>
    public int CoolTime { get; }
}
