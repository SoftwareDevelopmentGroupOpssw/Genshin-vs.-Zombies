using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 像豌豆射手一样的植物：间隔固定时间发射一发子弹，向前方一整条线中的敌人攻击
/// </summary>
public abstract class PeashooterBehaviour : Plant
{
    /// <summary>
    /// 发射的子弹的元素类型
    /// </summary>
    protected abstract Elements Element { get; }
    /// <summary>
    /// 发射的子弹的数据信息
    /// </summary>
    protected abstract IFlyerData Bullet { get; }
    /// <summary>
    /// 自动设置的数据
    /// </summary>
    public override IGameobjectData Data { get; set; }
    /// <summary>
    /// 发射间隔时间（毫秒）
    /// </summary>
    protected abstract int TimeDistance { get; }

    protected CountDown countdown;
    
    protected PeashooterBehaviour()
    {
        countdown = new CountDown(TimeDistance);
    }
    protected void Update()
    {
        if (countdown.Available)
        {
            Debug.Log(Bullet is null);
            Flyer flyer = GameController.Instance.AddFlyer(Bullet, transform.position);
            flyer.AvailableArea = new FrontLine();//子弹的范围为前一行  
            countdown.StartCountDown();
            CustomUpdate();
        }
    }
    /// <summary>
    /// 自定义更新，会在触发攻击召唤子弹时调用
    /// </summary>
    protected virtual void CustomUpdate()
    {

    }
}
