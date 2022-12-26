using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
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
}
