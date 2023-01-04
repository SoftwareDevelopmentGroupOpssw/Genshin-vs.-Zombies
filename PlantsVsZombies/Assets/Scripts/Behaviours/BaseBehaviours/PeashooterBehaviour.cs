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
    /// 发射的子弹的预制体的名字
    /// 在发射子弹时会用这个名字到FlyerPrefabSerializer中查找对应子弹数据
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
            if(countdown == null)
                countdown = new CountDown(timeDistance);
            if (countdown.MilisecondsCountDown != timeDistance)
                countdown.MilisecondsCountDown = timeDistance;
            return countdown;
        }
    }

    private DefaultHandler handler;
    public override IEffectHandler Handler
    {
        get
        {
            if (handler == null)
                handler = new DefaultHandler(Data);
            return handler;
        }
    }
    protected virtual void Start()
    {
        Data.AddOnReceiveAllDamageListener((damage) => PlayDamageingEffect());
    }
    AudioSource lastSource;
    private void PlayDamageingEffect()
    {
        float replayPercent = 0.5f;//当播放进度达到总时长的一定百分比就开始重新播放
        if (lastSource == null || !lastSource.gameObject.activeSelf || lastSource.time > lastSource.clip.length * replayPercent)//被塞到池子里去了，播放停止了
             lastSource = AudioManager.Instance.PlayRandomEffectAudio("chomp1", "chomp2");
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
        CommonFlyerData data = FlyerPrefabSerializer.Instance.GetFlyerData<CommonFlyerData>(bulletName);
        //获取子弹身上的脚本
        GameController.Instance.FlyersController.AddFlyer<Bullet>(data, transform.position, (bullet)=>
        {
            bullet.AvailableArea = new FrontLine();//子弹的范围为前一行  
            bullet.ElementType = element;//改变子弹的元素伤害
            bullet.Damage = Data.AtkPower;//子弹的伤害与攻击者的攻击力相同
            bullet.CanAddElement = true;//子弹一直可以附着元素
        });
        AudioManager.Instance.PlayRandomEffectAudio("throw1", "throw2");
    }
}
