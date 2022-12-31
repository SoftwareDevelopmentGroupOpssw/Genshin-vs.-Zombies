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
    /// <summary>
    /// ÿ�θ���ʱ�ı�Ԫ�ص�˳��
    /// �����û�и��µ�Ԫ�ص����¸��µ�Ԫ������
    /// </summary>
    private List<Elements> changeOrder = new List<Elements>(ELEMENTS_COUNT);
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
