using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 效果处理接口
/// </summary>
public interface IEffectHandler
{
    /// <summary>
    /// 自定义启用某个效果
    /// </summary>
    /// <param name="effect">效果</param>
    public void EnableEffect(IEffect effect);
    /// <summary>
    /// 自定义在效果持续时间中更新效果
    /// </summary>
    /// <param name="effect">效果</param>
    public void UpdateEffect(IEffect effect);
    /// <summary>
    /// 自定义移除效果
    /// </summary>
    /// <param name="effect">效果</param>
    public void DisableEffect(IEffect effect);
    /// <summary>
    /// 自定义对一列效果进行分析（启用、更新还是移除）
    /// </summary>
    /// <param name="effects"></param>
    public void CheckEffect(List<IEffect> effects);
}
