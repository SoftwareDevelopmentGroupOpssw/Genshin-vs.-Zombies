using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 负责植物数据储存的模块
/// 存储着所有植物的信息
/// </summary>
public class FlyerSerializer : Singleton<FlyerSerializer>
{
    private const string FLYERDATA_LOCATION = "SO/FlyerDatabase";
    private FlyerDatabase database;
    private FlyerDatabase Database
    {
        get
        {
            if (database == null)
            {
                database = ResourceManager.Instance.Load<FlyerDatabase>(FLYERDATA_LOCATION);
            }
            return database;
        }
    }
    IFlyerData RealGetFlyer(FlyerDatabase.Flyer flyer)
    {
        switch (flyer.Id)
        {
            case 1://莫娜的水弹
                return new PeaBulletData(flyer.Prefab, Elements.Water, 10);
        }
        return null;
    }
    /// <summary>
    /// 获取一个指定名字的飞行物
    /// </summary>
    /// <param name="name">飞行物名字</param>
    /// <returns>飞行物数据</returns>
    public IFlyerData GetFlyerData(string name)
    {
        foreach(var item in Database)
        {
            if(item.Name == name)
            {
                return RealGetFlyer(item);
            }
        }
        return null;
    }
}
