using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 负责植物数据储存的模块
/// 存储着所有植物的信息
/// </summary>
public class FlyerPrefabSerializer : Singleton<FlyerPrefabSerializer>
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
            //id 0~7都是豌豆子弹 ——只有元素类型发生了改变，体现在了脚本之中，因此数据上没有区别
            case 0://物理子弹
            case 1://水弹
            case 2://火弹
            case 3://冰弹
            case 4://雷弹
            case 5://风弹
            case 6://岩弹
            case 7://草弹
                return new PeaBulletData(flyer.Prefab);
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
        Debug.LogWarning($"The flyer name of {name} cannot be found in database. Please check the database at {FLYERDATA_LOCATION}");
        return null;
    }
    /// <summary>
    /// 获取一个指定名字的飞行物
    /// </summary>
    /// <typeparam name="T">飞行物数据的类型</typeparam>
    /// <param name="name">飞行物名字</param>
    /// <returns>飞行物数据</returns>
    public T GetFlyerData<T>(string name) where T : IFlyerData
    {
        return (T)GetFlyerData(name);
    }
}
