using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 像豌豆射手一样的植物：间隔固定时间发射一发子弹，向前方一整条线中的敌人攻击
/// </summary>
public class PeashooterBehaviour : Plant
{
    [Header("发射的子弹的元素类型")]
    [SerializeField]
    private Elements element = Elements.None;
    /// <summary>
    /// 发射的子弹的元素类型
    /// </summary>
    public Elements Element { get => element; set => element = value; }

    [Header("发射的子弹的预制体的名字")]
    [SerializeField]
    private string bulletName = "PhysicalBullet";
    /// <summary>
    /// 发射的子弹的名字
    /// </summary>
    public string BulletName { get => bulletName; set => bulletName = value; }


    [Header("发射子弹的间隔（毫秒）")]
    [SerializeField]
    private int timeDistance = 2000;
    /// <summary>
    /// 发射间隔时间（毫秒）
    /// </summary>
    public int TimeDistance { get => timeDistance; set => timeDistance = value; }




    private CountDown countdown;
    protected CountDown CountDown
    {
        get
        {
            if (countdown.MilisecondsCountDown != timeDistance)
                countdown.MilisecondsCountDown = timeDistance;
            return countdown;
        }
    }
    
    public PeashooterBehaviour()
    {
        countdown = new CountDown(timeDistance);
    }
    /// <summary>
    /// 是否有怪物在攻击距离中
    /// </summary>
    /// <returns></returns>
    protected bool HaveMonster()
    {
        Area area = new FrontLine();
        Vector2Int nowPos = GameController.Instance.WorldToGrid(transform.position);
        return GameController.Instance.MonstersController.HaveMonster(area, nowPos);
    }
    /// <summary>
    /// 发射子弹
    /// </summary>
    protected void ReleasePeaBullet()
    {
        //获取子弹的贴图信息
        PeaBulletData data = FlyerPrefabSerializer.Instance.GetFlyerData<PeaBulletData>(bulletName);
        //获取子弹身上的脚本
        PeaBulletBehaviour flyer = GameController.Instance.FlyersController.AddFlyer<PeaBulletBehaviour>(data, transform.position);
        flyer.AvailableArea = new FrontLine();//子弹的范围为前一行  
        flyer.ElementType = element;//改变子弹的元素伤害
        flyer.AtkDmg = Data.AtkPower;//豌豆的伤害与攻击者的攻击力相同
        flyer.CanAddElement = true;//豌豆一直可以附着元素

    }
}
