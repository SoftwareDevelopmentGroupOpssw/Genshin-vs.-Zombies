using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装载着特定类型物品，可以通过元素来存取
/// </summary>
/// <typeparam name="T">类型</typeparam>
public class ElementalObject<T> : IEnumerable<KeyValuePair<Elements,T>>
{
    private readonly static int ELEMENTS_COUNT = System.Enum.GetValues(typeof(Elements)).Length;
    private T[] items = new T[ELEMENTS_COUNT];
    public ElementalObject()
    {
        items.Initialize();
    }
    /// <summary>
    /// 索引器
    /// </summary>
    /// <param name="element">元素类型</param>
    /// <returns>物体</returns>
    public T this[Elements element]
    {
        get
        {
            return items[(int)element];
        }
        set
        {
            items[(int)element] = value;
        }
    }
    /// <summary>
    /// 迭代器
    /// </summary>
    /// <returns></returns>
    public IEnumerator<KeyValuePair<Elements,T>> GetEnumerator()
    {
        for (int i = 0; i < ELEMENTS_COUNT; i++)
            yield return new KeyValuePair<Elements, T>((Elements)i, items[i]);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
