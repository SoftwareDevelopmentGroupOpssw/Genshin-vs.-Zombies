using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ֲ�����ݴ����ģ��
/// �洢������ֲ�����Ϣ
/// </summary>
public class PlantPrefabSerializer : Singleton<PlantPrefabSerializer>, IEnumerable<IPlantData>
{
    private const string PLANTDATA_LOCATION = "SO/PlantDatabase";
    private PlantDatabase database;
    private PlantDatabase Database
    {
        get
        {
            if (database == null)
            {
                database = ResourceManager.Instance.Load<PlantDatabase>(PLANTDATA_LOCATION);
            }
            return database;
        }
    }
    IPlantData GetPlantData(PlantDatabase.Plant data)
    {
        switch (data.Id)
        {
            case 0:
                return new YanfeiData(data.Prefab, data.CardSprite);
            case 1:
                return new MonaData(data.Prefab, data.CardSprite);
            case 2:
                return new LisaData(data.Prefab, data.CardSprite);
            case 3:
                return new NahidaData(data.Prefab, data.CardSprite);
            case 4:
                return new SucroseData(data.Prefab, data.CardSprite);
            case 5:
                return new NingguangData(data.Prefab, data.CardSprite);
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
        IPlantData data = GetPlantData(Database.GetData(plantName));
        if (data == null)
            Debug.LogWarning($"The plant name of {plantName} cannot be found in database. Please check the database at {PLANTDATA_LOCATION}");
        return data;
    }
    /// <summary>
    /// ��ȡһ����Ӧ���ֵ�ֲ��������ʵ��
    /// </summary>
    /// <typeparam name="T">ֲ�����ݵ�����</typeparam>
    /// <param name="plantName">ֲ������</param>
    /// <returns>ֲ������</returns>
    public T GetPlantData<T>(string plantName) where T: IPlantData
    {
        return (T)GetPlantData(plantName);
    } 


    public IEnumerator<IPlantData> GetEnumerator()
    {
        foreach (var item in Database)
        {
            yield return GetPlantData(item);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
