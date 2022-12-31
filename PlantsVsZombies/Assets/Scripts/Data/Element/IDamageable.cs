using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可以被伤害的物体
/// </summary>
public interface IDamageable
{
    /// <summary>
    /// 获取一个伤害接收器来接受伤害
    /// </summary>
    /// <returns>伤害接收器</returns>
    public IDamageReceiver GetReceiver();
}
