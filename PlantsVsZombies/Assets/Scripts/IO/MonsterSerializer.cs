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
                return new CommonZombieData(data.Prefab); //ҡ�콩ʬʹ��ͬ��������
        }
        return null;
    }
    /// <summary>
    /// ��ȡһ����Ӧ���ֵ�ħ��������ʵ��
    /// </summary>
    /// <param name="monsterName">ħ������</param>
    /// <returns>ֲ������</returns>
    public IMonsterData GetMonsterData(string monsterName)
    {
        IMonsterData data = RealGetData(Database.GetData(monsterName));
        if (data == null)
            Debug.LogWarning($"The monster name of {monsterName} cannot be found in database. Please check the database at {MONSTERDATA_LOCATION}");
        return data;
    }
    /// <summary>
    /// ��ȡһ����Ӧ���ֵ�ħ��������ʵ��
    /// </summary>
    /// <typeparam name="T">�������ݵ�����</typeparam>
    /// <param name="monsterName">ħ������</param>
    /// <returns>ħ������</returns>
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
