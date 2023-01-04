using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPrefabSerializer : Singleton<MonsterPrefabSerializer>, IEnumerable<IMonsterData>
{
    private const string MONSTERDATA_LOCATION = "SO/MonsterDatabase";
    private MonsterDatabase database;
    private MonsterDatabase Database
    {
        get
        {
            if (database == null)
            {
                database = ResourceManager.Instance.Load<MonsterDatabase>(MONSTERDATA_LOCATION);
            }
            return database;
        }
    }
    IMonsterData RealGetData(MonsterDatabase.Monster data)
    {
        switch (data.Id)
        {
            case 0:
                return new CommonZombieData(data.Prefab);
            case 1:
                return new RoadConeZombieData(data.Prefab);
            case 2:
                return new BucketHeadZombieData(data.Prefab);
            case 3:
                return new CommonZombieData(data.Prefab); //摇旗僵尸使用同样的数据
        }
        return null;
    }
    /// <summary>
    /// 获取一个对应名字的魔物数据新实例
    /// </summary>
    /// <param name="monsterName">魔物名字</param>
    /// <returns>植物数据</returns>
    public IMonsterData GetMonsterData(string monsterName)
    {
        IMonsterData data = RealGetData(Database.GetData(monsterName));
        if (data == null)
            Debug.LogWarning($"The monster name of {monsterName} cannot be found in database. Please check the database at {MONSTERDATA_LOCATION}");
        return data;
    }
    /// <summary>
    /// 获取一个对应名字的魔物数据新实例
    /// </summary>
    /// <typeparam name="T">怪物数据的类型</typeparam>
    /// <param name="monsterName">魔物名字</param>
    /// <returns>魔物数据</returns>
    public T GetMonsterData<T>(string monsterName) where T : IMonsterData
    {
        return (T)GetMonsterData(monsterName);
    }


    public IEnumerator<IMonsterData> GetEnumerator()
    {
        foreach (var item in Database)
        {
            yield return RealGetData(item);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
