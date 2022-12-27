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
    /// 添加元素附着
    /// 此方法添加的元素不会触发元素反应
    /// </summary>
    /// <param name="element"></param>
    public void AddElement(Elements element);
    /// <summary>
    /// 移除元素附着
    /// 此方法移除的元素不会触发事件
    /// </summary>
    /// <param name="element"></param>
    public void RemoveElement(Elements element);
    
    
    
    
    /// <summary>
    /// 从目标处受到伤害
    /// 此函数会计算抗性、伤害、反应，同时进行一些事件的触发
    /// </summary>
    /// <param name="damage">受到伤害的来源</param>
    public void ReceiveDamage(IElementalDamage damage);

    
    
    /// <summary>
    /// 添加受到伤害监听
    /// </summary>
    /// <param name="element">受到伤害的元素类型</param>
    /// <param name="action">受到伤害时调用的函数</param>
    public void AddOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action);
    /// <summary>
    /// 移除受到伤害监听
    /// </summary>
    /// <param name="element">受到伤害的元素类型</param>
    /// <param name="action">受到伤害时调用的函数</param>
    public void RemoveOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action);

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

    /// <summary>
    /// 添加元素反应监听
    /// </summary>
    /// <param name="element">元素类型</param>
    /// <param name="action">元素反应函数</param>
    public void AddOnElementReactedListener(Elements element, System.Action<ElementsReaction> action);
    /// <summary>
    /// 移除元素反应监听
    /// </summary>
    /// <param name="element">元素类型</param>
    /// <param name="action">元素反应函数</param>
    public void RemoveOnElementReactedListener(Elements element, System.Action<ElementsReaction> action);

}
