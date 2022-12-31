using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔物数据和魔物预制体SO文件
/// </summary>
[CreateAssetMenu(fileName = "MonsterDataCollection", menuName = "SODatabase/MonsterDatabaseSO")]
public class MonsterDatabase : ScriptableObject, IEnumerable<MonsterDatabase.Monster>
{
    [SerializeField]
    private List<Monster> monsters = new List<Monster>();

    /// <summary>
    /// 以指定的名字获取一个数据
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
