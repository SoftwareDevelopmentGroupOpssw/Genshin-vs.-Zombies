using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// װ�����ض�������Ʒ������ͨ��Ԫ������ȡ
/// </summary>
/// <typeparam name="T">����</typeparam>
public class ElementalObject<T> : IEnumerable<KeyValuePair<Elements,T>>
{
    private readonly static int ELEMENTS_COUNT = System.Enum.GetValues(typeof(Elements)).Length;
    private T[] items = new T[ELEMENTS_COUNT];
    public ElementalObject()
    {
        items.Initialize();
    }
    /// <summary>
    /// ������
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <returns>����</returns>
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
    /// ������
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
