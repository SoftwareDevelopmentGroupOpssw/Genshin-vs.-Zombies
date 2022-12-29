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
    /// ����
    /// </summary>
    /// <param name="match">���ϵ�����</param>
    /// <returns>��������������</returns>
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
