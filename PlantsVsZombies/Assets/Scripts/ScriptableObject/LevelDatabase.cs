using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// װ�عؿ���ͼ�͹ؿ����ݵ�SO�ļ�
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
    /// �Ѽ��صĹؿ���������
    /// </summary>
    public int Count => levels.Count;

    /// <summary>
    /// �������洢���ǹؿ����ݵĶ���
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
