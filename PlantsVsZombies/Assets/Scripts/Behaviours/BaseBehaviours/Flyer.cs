using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 飞行物脚本基类
/// </summary>
public abstract class Flyer : BaseGameobject
{
    // Start is called before the first frame update
    /// <summary>
    /// 飞行物能够到达的区域
    /// </summary>
    public Area AvailableArea { get; set; }
    /// <summary>
    /// 飞行物数据
    /// </summary>
    public IFlyerData Data { get; set; }
}
