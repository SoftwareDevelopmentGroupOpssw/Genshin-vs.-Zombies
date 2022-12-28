using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pvz中的游戏物体（植物、飞行物、魔物）
/// </summary>
public abstract class BaseGameobject : MonoBehaviour
{
    /// <summary>
    /// 物体数据
    /// </summary>
    public abstract IGameobjectData Data { get; set; }
}
