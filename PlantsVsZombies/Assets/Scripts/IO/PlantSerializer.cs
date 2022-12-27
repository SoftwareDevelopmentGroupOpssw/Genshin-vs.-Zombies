using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责植物数据储存的模块
/// 存储着所有植物的信息
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
    /// 获取一个对应名字的植物数据新实例
    /// </summary>
    /// <param name="plantName">植物名字</param>
    /// <returns>植物数据</returns>
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
