using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 伤害接受器：自动计算元素反应和元素伤害
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
    /// <returns>造成伤害的结果</returns>
    public bool ReceiveDamage(IElementalDamage damage);

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
    /// 为所有类型的元素伤害添加监听
    /// </summary>
    /// <param name="action">受到伤害时调用的函数</param>
    public void AddOnReceiveAllDamageListener(System.Action<IElementalDamage> action);
    /// <summary>
    /// 为所有类型的元素伤害移除监听
    /// </summary>
    /// <param name="action">受到伤害时调用的函数</param>
    public void RemoveOnReceiveAllDamageListener(System.Action<IElementalDamage> action);

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
