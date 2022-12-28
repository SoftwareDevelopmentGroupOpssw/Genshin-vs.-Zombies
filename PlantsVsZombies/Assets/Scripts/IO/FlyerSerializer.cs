using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����ֲ�����ݴ����ģ��
/// �洢������ֲ�����Ϣ
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
            case 1://Ī�ȵ�ˮ��
                return new PeaBulletData(flyer.Prefab, Elements.Water, 10);
        }
        return null;
    }
    /// <summary>
    /// ��ȡһ��ָ�����ֵķ�����
    /// </summary>
    /// <param name="name">����������</param>
    /// <returns>����������</returns>
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
