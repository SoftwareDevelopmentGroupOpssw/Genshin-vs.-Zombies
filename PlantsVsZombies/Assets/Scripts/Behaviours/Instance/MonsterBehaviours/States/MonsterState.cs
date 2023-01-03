using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 魔物状态基类
/// </summary>
public class MonsterState
{
    protected MonsterState() { }
    /// <summary>
    /// 进入状态时调用
    /// </summary>
    public virtual void OnEnterState() { }
    /// <summary>
    /// 在此状态时帧更新函数
    /// </summary>
    public virtual void Update() { }
    /// <summary>
    /// 退出状态时调用
    /// </summary>
    public virtual void OnExitState() { }
}
