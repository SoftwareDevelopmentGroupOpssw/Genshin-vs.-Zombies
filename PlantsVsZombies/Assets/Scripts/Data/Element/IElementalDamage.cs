using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 元素伤害接口
/// </summary>
public interface IElementalDamage
{
    /// <summary>
    /// 伤害大小
    /// </summary>
    public int AtkDmg { get; }
    /// <summary>
    /// 伤害类型
    /// </summary>
    public Elements ElementType { get; }
    /// <summary>
    /// 此次伤害能否施加元素
    /// </summary>
    public bool CanAddElement { get; }
}
