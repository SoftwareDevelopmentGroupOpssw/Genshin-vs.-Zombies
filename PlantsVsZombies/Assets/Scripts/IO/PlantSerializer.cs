using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ֲ�����ݴ����ģ��
/// �洢������ֲ�����Ϣ
/// </summary>
public class PlantSerializer:Singleton<PlantSerializer>,IEnumerable<IPlantData>
{
    private const string PLANTDATA_LOCATION = "SO/PlantDatabase";
    private PlantDatabase database;
    IPlantData GetPlantData(PlantDatabase.Plant data)
    {
        switch (data.Id)
        {
            case 1:
                return new MonaData(data.Prefab, data.CardSprite);
        }
        return null;
    }
    /// <summary>
    /// ��ȡһ����Ӧ���ֵ�ֲ��������ʵ��
    /// </summary>
    /// <param name="plantName">ֲ������</param>
    /// <returns>ֲ������</returns>
    public IPlantData GetPlantData(string plantName)
    {
        if (database == null)
        {
            database = ResourceManager.Instance.Load<PlantDatabase>(PLANTDATA_LOCATION);
        }
        return GetPlantData(database.GetData(plantName));
    }

    public IEnumerator<IPlantData> GetEnumerator()
    {
        foreach(var item in database)
        {
            yield return GetPlantData(item);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new System.NotImplementedException();
    }
}
