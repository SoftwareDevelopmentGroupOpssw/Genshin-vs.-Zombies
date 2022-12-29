using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����ֲ�����ݴ����ģ��
/// �洢������ֲ�����Ϣ
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
            //id 0~7�����㶹�ӵ� ����ֻ��Ԫ�����ͷ����˸ı䣬�������˽ű�֮�У����������û������
            case 0://�����ӵ�
            case 1://ˮ��
            case 2://��
            case 3://����
            case 4://�׵�
            case 5://�絯
            case 6://�ҵ�
            case 7://�ݵ�
                return new PeaBulletData(flyer.Prefab);
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
        Debug.LogWarning($"The flyer name of {name} cannot be found in database. Please check the database at {FLYERDATA_LOCATION}");
        return null;
    }
    /// <summary>
    /// ��ȡһ��ָ�����ֵķ�����
    /// </summary>
    /// <typeparam name="T">���������ݵ�����</typeparam>
    /// <param name="name">����������</param>
    /// <returns>����������</returns>
    public T GetFlyerData<T>(string name) where T : IFlyerData
    {
        return (T)GetFlyerData(name);
    }
}
