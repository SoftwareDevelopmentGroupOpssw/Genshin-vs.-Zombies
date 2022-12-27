using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ֲ�����ݺ���ͼ��ϢSO�ļ�
/// </summary>
[CreateAssetMenu(fileName = "PlantDataCollection", menuName = "PlantDatabaseSO")]
public class PlantDatabase : ScriptableObject, IEnumerable<PlantDatabase.Plant>
{
    [SerializeField]
    private List<Plant> plants = new List<Plant>();

    /// <summary>
    /// �ø�����������Ѱ�������ֵ�ֲ������
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Plant GetData(string name) => plants.Find((plant) => plant.Name == name);

    /// <summary>
    /// ����
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
