using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ħ�����ݺ�ħ��Ԥ����SO�ļ�
/// </summary>
[CreateAssetMenu(fileName = "MonsterDataCollection", menuName = "SODatabase/MonsterDatabaseSO")]
public class MonsterDatabase : ScriptableObject, IEnumerable<MonsterDatabase.Monster>
{
    [SerializeField]
    private List<Monster> monsters = new List<Monster>();

    /// <summary>
    /// ��ָ�������ֻ�ȡһ������
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Monster GetData(string name)
    {
        return monsters.Find((monster) => monster.Name == name);
    }

    public IEnumerator<Monster> GetEnumerator()
    {
        return monsters.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [System.Serializable]
    public struct Monster
    {
        public GameObject Prefab;
        public string Name;
        public int Id;
    }
}
