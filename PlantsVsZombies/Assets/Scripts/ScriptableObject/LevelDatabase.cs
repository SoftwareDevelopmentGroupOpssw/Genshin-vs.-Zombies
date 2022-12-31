using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 装载关卡贴图和关卡数据的SO文件
/// </summary>
[CreateAssetMenu(fileName = "LevelDataCollection", menuName = "SODatabase/LevelDatabaseSO")]
public class LevelDatabase : ScriptableObject,IEnumerable<LevelDatabase.Level>
{
    [SerializeField]
    private List<Level> levels = new List<Level>();
    [System.Serializable]
    public struct Level
    {
        public Sprite Sprite;
        public string Name;
        public int Id;
    }
    /// <summary>
    /// 已加载的关卡数据数量
    /// </summary>
    public int Count => levels.Count;

    /// <summary>
    /// 遍历，存储的是关卡数据的队列
    /// </summary>
    /// <returns></returns>
    public IEnumerator<Level> GetEnumerator()
    {
        return levels.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
