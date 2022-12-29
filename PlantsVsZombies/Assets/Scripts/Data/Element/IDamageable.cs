using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可以被伤害的物体
/// </summary>
public interface IDamageable
{
    public IDamageReceiver GetReceiver();
}
