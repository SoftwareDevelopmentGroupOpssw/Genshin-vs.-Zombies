using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 元素反应基类
/// </summary>
public abstract class ElementsReaction
{
    /// <summary>
    /// 系统物体
    /// </summary>
    protected static IGameobjectData system = SystemObject.Instance;
    
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
        switch (before)
        {
            case Elements.Water:
                switch (after)
                {
                    case Elements.Fire:
                        return new Vaporize();
                    case Elements.Ice:
                        return new Frozen();
                    case Elements.Electric:
                        return new ElectroCharged();
                    case Elements.Wind:
                        return new Swirl();
                    case Elements.Stone:
                        return new Crystallize();
                    case Elements.Grass:
                        return new Bloom();
                }
                break;
            case Elements.Fire:
                switch (after)
                {
                    case Elements.Water:
                        return new Vaporize();
                    case Elements.Ice:
                        return new Melt();
                    case Elements.Electric:
                        return new Overloaded();
                    case Elements.Wind:
                        return new Swirl();
                    case Elements.Stone:
                        return new Crystallize();
                    case Elements.Grass:
                        return new Burning();
                }
                break;
            case Elements.Ice:
                switch (after)
                {
                    case Elements.Water:
                        return new Frozen();
                    case Elements.Fire:
                        return new Melt();
                    case Elements.Electric:
                        return new SuperConduct();
                    case Elements.Wind:
                        return new Swirl();
                    case Elements.Stone:
                        return new Crystallize();
                }
                break;
            case Elements.Electric:
                switch (after)
                {
                    case Elements.Water:
                        return new ElectroCharged();
                    case Elements.Fire:
                        return new Overloaded();
                    case Elements.Ice:
                        return new SuperConduct();
                    case Elements.Wind:
                        return new Swirl();
                    case Elements.Stone:
                        return new Crystallize();
                    case Elements.Grass:
                        return new Quicken();
                }
                break;
            //风元素和岩元素不能被附着，前元素不能是这两个
            case Elements.Wind:
            case Elements.Stone:
                break;
            case Elements.Grass:
                switch (after)
                {
                    case Elements.Water:
                        return new Bloom();
                    case Elements.Fire:
                        return new Burning();
                    case Elements.Electric:
                        return new Quicken();
                }
                break;
        }
        return null;
    }
    /// <summary>
    /// 元素反应名字
    /// </summary>
    public abstract string ReactionName { get; }
    /// <summary>
    /// 元素反应释放
    /// </summary>
    /// <param name="damage">元素伤害来源</param>
    /// <param name="target">元素反应目标</param>
    public abstract void Action(IElementalDamage damage, IMonsterData target);
}
