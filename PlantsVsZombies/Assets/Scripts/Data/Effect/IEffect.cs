using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 效果状态
/// </summary>
public enum EffectState
{
    /// <summary>
    /// 效果刚被添加，尚未触发
    /// </summary>
    Initialized,
    /// <summary>
    /// 效果已经被添加，正在触发
    /// </summary>
    Processing,
    /// <summary>
    /// 效果结束，等待被删除
    /// </summary>
    End,
    /// <summary>
    /// 效果在执行中出错
    /// </summary>
    Error,
}
/// <summary>
/// 效果接口
/// </summary>
public interface IEffect
{
    /// <summary>
    /// 效果名
    /// 相同类型的效果（比如都是移动速度衰减效果）名字相同
    /// </summary>
    public string EffectName { get; }
    /// <summary>
    /// 这个效果所处的状态
    /// </summary>
    public EffectState State { get; }
    /// <summary>
    /// 施加者
    /// </summary>
    public IGameobjectData Caster { get; }

    /// <summary>
    /// 执行效果
    /// </summary>
    /// <param name="target">效果对象</param>
    public void EnableEffect(IGameobjectData target);
    /// <summary>
    /// 移除效果
    /// </summary>
    /// <param name="target">效果对象</param>
    public void DisableEffect(IGameobjectData target);
    /// <summary>
    /// 帧更新调用函数
    /// </summary>
    /// <param name="target">对象</param>
    public void UpdateEffect(IGameobjectData target);
}
