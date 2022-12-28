using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责关卡数据存储的模块
/// 使用此模块的方法来获取信息
/// </summary>
public class LevelSerializer:Singleton<LevelSerializer>,IEnumerable<KeyValuePair<string,ILevelData>>
{
    private const string LEVELDATA_LOCATION = "SO/LevelDatabase";
    private LevelDatabase database;
    private Dictionary<string, ILevelData> levelDic = new Dictionary<string, ILevelData>();
    ILevelData GetLevelData(int levelId,Sprite sprite)
    {
        ILevelData data = null;
        switch (levelId)
        {
            case 0:
                data = new Level1(sprite);
                break;
        }
        return data;
    }
    void Initialize()
    {
        database = ResourceManager.Instance.Load<LevelDatabase>(LEVELDATA_LOCATION);
        foreach (var item in database)
        {
            levelDic.Add(item.Name, GetLevelData(item.Id, item.Sprite));
        }
    }
    public ILevelData GetLevel(string levelName)
    {
        if(database == null)
        {
            Initialize();
        }
        return levelDic[levelName];
    }

    public IEnumerator<KeyValuePair<string, ILevelData>> GetEnumerator()
    {
        if (database == null)
            Initialize();
        return levelDic.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        if (database == null)
            Initialize();
        return levelDic.GetEnumerator();
    }
}
