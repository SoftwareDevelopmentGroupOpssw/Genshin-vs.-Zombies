using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通用飞行物数据
/// </summary>
public class CommonFlyerData : IFlyerData
{
    public CommonFlyerData(GameObject original)
    {
        OriginalReference = original;
    }

    public GameObject GameObject { get; set; }

    public GameObject OriginalReference { get; private set; }

    
    /// <summary>
    /// 拓展：豌豆可以加效果
    /// </summary>
    /// <param name="effect"></param>
    public void AddEffect(IEffect effect)
    {
        
    }
    /// <summary>
    /// 拓展：豌豆可以获得所有的效果列表
    /// </summary>
    /// <returns></returns>
    public List<IEffect> GetEffects()
    {
        return null;
    }
    /// <summary>
    /// 拓展：豌豆可以移除效果
    /// </summary>
    /// <param name="effect"></param>
    public void RemoveEffect(IEffect effect)
    {
        
    }
    public void Dispose()
    {

    }
}
