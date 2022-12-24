using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 元素反应基类
/// </summary>
public abstract class ElementsReaction : MonoBehaviour
{
    /// <summary>
    /// 获取一个元素反应
    /// </summary>
    /// <param name="before">已经附着的元素</param>
    /// <param name="after">即将附着的元素</param>
    /// <returns>元素反应，如果此元素反应不存在则为空</returns>
    public static ElementsReaction GetReaction(Elements before, Elements after)
    {
        //TODO:对于每种可行的元素反应，返回一个对应的元素反应出去
        //这个元素反应显然是需要继承ElementsReaction类并重写Action方法
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// 元素反应释放
    /// </summary>
    /// <param name="damage">元素伤害来源</param>
    /// <param name="target">元素反应目标</param>
    public abstract void Action(IElementalDamage damage, IMonsterData target);
}
