using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���㶹����һ����ֲ�����̶�ʱ�䷢��һ���ӵ�����ǰ��һ�������еĵ��˹���
/// </summary>
public abstract class PeashooterBehaviour : Plant
{
    /// <summary>
    /// ������ӵ���Ԫ������
    /// </summary>
    protected abstract Elements Element { get; }
    /// <summary>
    /// ������ӵ���������Ϣ
    /// </summary>
    protected abstract IFlyerData Bullet { get; }
    /// <summary>
    /// �Զ����õ�����
    /// </summary>
    public override IGameobjectData Data { get; set; }
    /// <summary>
    /// ������ʱ�䣨���룩
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
            flyer.AvailableArea = new FrontLine();//�ӵ��ķ�ΧΪǰһ��  
            countdown.StartCountDown();
            CustomUpdate();
        }
    }
    /// <summary>
    /// �Զ�����£����ڴ��������ٻ��ӵ�ʱ����
    /// </summary>
    protected virtual void CustomUpdate()
    {

    }
}
