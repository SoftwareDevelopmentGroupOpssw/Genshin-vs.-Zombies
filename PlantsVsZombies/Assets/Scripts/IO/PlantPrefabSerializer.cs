using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责植物数据储存的模块
/// 存储着所有植物的信息
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
    /// 获取一个对应名字的植物数据新实例
    /// </summary>
    /// <param name="plantName">植物名字</param>
    /// <returns>植物数据</returns>
    public IPlantData GetPlantData(string plantName)
    {
        IPlantData data = GetPlantData(Database.GetData(plantName));
        if (data == null)
            Debug.LogWarning($"The plant name of {plantName} cannot be found in database. Please check the database at {PLANTDATA_LOCATION}");
        return data;
    }
    /// <summary>
    /// 获取一个对应名字的植物数据新实例
    /// </summary>
    /// <typeparam name="T">植物数据的类型</typeparam>
    /// <param name="plantName">植物名字</param>
    /// <returns>植物数据</returns>
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
