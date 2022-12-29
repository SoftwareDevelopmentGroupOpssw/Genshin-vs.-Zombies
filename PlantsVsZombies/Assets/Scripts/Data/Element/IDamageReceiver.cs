using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 能接受伤害的接口
/// </summary>
public interface IDamageReceiver : ICharactorData
{
    /// <summary>
    /// 添加元素
    /// </summary>
    /// <param name="element"></param>
    public void AddElement(Elements element);
    /// <summary>
    /// 移除元素
    /// </summary>
    /// <param name="element"></param>
    public void RemoveElement(Elements element);

    /// <summary>
    /// 获得所有元素附着
    /// </summary>
    /// <returns></returns>
    public Elements[] GetAllElements();

    /// <summary>
    /// 受到元素伤害
    /// </summary>
    /// <param name="damage"></param>
    public void ReceiveDamage(IElementalDamage damage);

    /// <summary>
    /// 添加受到元素伤害监听
    /// </summary>
    /// <param name="element"></param>
    /// <param name="action"></param>
    public void AddOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action);
    /// <summary>
    /// 移除受到伤害监听
    /// </summary>
    /// <param name="element"></param>
    /// <param name="action"></param>
    public void RemoveOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action);
    /// <summary>
    /// 添加元素反应监听
    /// </summary>
    /// <param name="element">监听的元素</param>
    /// <param name="action">元素消失时触发的反应</param>
    public void AddOnElementReactedListener(Elements element, System.Action<ElementsReaction> action);
    /// <summary>
    /// 移除元素反应监听
    /// </summary>
    /// <param name="element">监听的元素</param>
    /// <param name="action">元素消失时触发的反应</param>
    public void RemoveOnElementReactedListener(Elements element, System.Action<ElementsReaction> action);
}
