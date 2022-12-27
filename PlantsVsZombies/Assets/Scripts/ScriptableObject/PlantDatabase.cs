using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 植物数据和贴图信息SO文件
/// </summary>
[CreateAssetMenu(fileName = "PlantDataCollection", menuName = "PlantDatabaseSO")]
public class PlantDatabase : ScriptableObject, IEnumerable<PlantDatabase.Plant>
{
    [SerializeField]
    private List<Plant> plants = new List<Plant>();

    /// <summary>
    /// 用给定的名字找寻符合名字的植物数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Plant GetData(string name) => plants.Find((plant) => plant.Name == name);

    /// <summary>
    /// 遍历
    /// </summary>
    /// <returns></returns>
    public IEnumerator<Plant> GetEnumerator()
    {
        return plants.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [System.Serializable]
    public struct Plant
    {
        public Sprite CardSprite;
        public GameObject Prefab;
        public string Name;
        public int Id;
    }
}
