using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ؿ����ݴ洢��ģ��
/// ʹ�ô�ģ��ķ�������ȡ��Ϣ
/// </summary>
public class LevelSpriteSerializer:Singleton<LevelSpriteSerializer>,IEnumerable<KeyValuePair<string,ILevelData>>
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
        if (!levelDic.ContainsKey(levelName))
        {
            Debug.LogWarning($"The level name of {levelName} cannot be found in database. Please check the database at {LEVELDATA_LOCATION}");
            return null;
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
