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
    /// <summary>
    /// 每次更新时改变元素的顺序
    /// 由最久没有更新的元素到最新更新的元素排序
    /// </summary>
    private List<Elements> changeOrder = new List<Elements>(ELEMENTS_COUNT);
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
            if (!changeOrder.Contains(element))
                changeOrder.Add(element);
            else
            {
                changeOrder.Remove(element);
                changeOrder.Add(element);
            }
            items[(int)element] = value;
        }
    }
    /// <summary>
    /// 查找
    /// </summary>
    /// <param name="match">符合的条件</param>
    /// <returns>符合条件的数组</returns>
    public KeyValuePair<Elements,T>[] Find(System.Predicate<T> match)
    {
        List<KeyValuePair<Elements, T>> list = new List<KeyValuePair<Elements, T>>();
        for(int i = 0;i<items.Length;i++)
        {
            T item = items[i];
            if (match.Invoke(item))
                list.Add(new KeyValuePair<Elements, T>((Elements)i,item));
        }
        return list.ToArray();
    }
    /// <summary>
    /// 迭代器
    /// </summary>
    /// <returns></returns>
    public IEnumerator<KeyValuePair<Elements,T>> GetEnumerator()
    {
        bool[] outputBool = new bool[ELEMENTS_COUNT];
        for(int i = 0 ; i < changeOrder.Count; i++)
        {
            outputBool[(int)changeOrder[i]] = true;
            yield return new KeyValuePair<Elements, T>(changeOrder[i], items[(int)changeOrder[i]]);
        }
        for (int i = 0; i < ELEMENTS_COUNT; i++)
        {
            if(!outputBool[i])
                yield return new KeyValuePair<Elements, T>((Elements)i, items[i]);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
