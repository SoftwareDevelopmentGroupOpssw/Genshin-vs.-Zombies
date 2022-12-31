using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔物接口
/// </summary>
public interface IMonsterData:ICharactorData,IDamageReceiver
{
    /// <summary>
    /// 韧性值
    /// </summary>
    public int Strength { get; set; }

    /// <summary>
    /// 移动速度
    /// </summary>
    public int Speed { get; set; }
    
    /// <summary>
    /// 获取对某种元素的抗性
    /// </summary>
    /// <param name="element">元素类型</param>
    /// <returns>抗性值（0~1）</returns>
    public float GetResistance(Elements element);
    /// <summary>
    /// 设置对某种元素的抗性
    /// </summary>
    /// <param name="value">数值</param>
    /// <param name="element">元素类型</param>
    public void SetResistance(float value, Elements element);

    /// <summary>
    /// 添加元素附着监听
    /// </summary>
    /// <param name="element">元素附着</param>
    /// <param name="action">受到元素附着时调用的函数</param>
    public void AddOnAddElementListener(Elements element, System.Action action);
    /// <summary>
    /// 移除元素附着监听
    /// </summary>
    /// <param name="element">元素附着</param>
    /// <param name="action">受到元素附着时调用的函数</param>
    public void RemoveOnAddElementListener(Elements element, System.Action action);
}
