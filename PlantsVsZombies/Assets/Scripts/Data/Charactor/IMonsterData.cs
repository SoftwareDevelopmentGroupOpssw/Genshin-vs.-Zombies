using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔物接口
/// </summary>
public interface IMonsterData:ICharactorData
{
    /// <summary>
    /// 韧性值
    /// </summary>
    public int Strength { get; set; }
    /// <summary>
    /// 获取对某种元素的抗性
    /// </summary>
    /// <param name="element">元素类型</param>
    /// <returns>抗性值（0~1）</returns>
    public float GetResistance(Elements element);
    /// <summary>
    /// 从目标处受到伤害
    /// </summary>
    /// <param name="causer">受到伤害的来源</param>
    public void ReceiveDamage(IElementalDamage causer);
}
