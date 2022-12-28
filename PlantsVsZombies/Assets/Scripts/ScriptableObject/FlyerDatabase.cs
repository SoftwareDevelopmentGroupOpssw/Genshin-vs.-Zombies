using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlyerDataCollection",menuName = "SODatabase/FlyerDatabaseSO")]
public class FlyerDatabase : ScriptableObject, IEnumerable<FlyerDatabase.Flyer>
{
    [SerializeField]
    private List<Flyer> flyers = new List<Flyer>();

    /// <summary>
    /// �ø�����������Ѱ�������ֵķ���������
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Flyer GetData(string name) => flyers.Find((flyer) => flyer.Name == name);

    /// <summary>
    /// ����
    /// </summary>
    /// <returns></returns>
    public IEnumerator<Flyer> GetEnumerator()
    {
        return flyers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [System.Serializable]
    public struct Flyer
    {
        public GameObject Prefab;
        public string Name;
        public int Id;
    }
}
