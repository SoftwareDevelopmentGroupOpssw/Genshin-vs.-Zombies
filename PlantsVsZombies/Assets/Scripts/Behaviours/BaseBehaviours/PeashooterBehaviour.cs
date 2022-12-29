using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���㶹����һ����ֲ�����̶�ʱ�䷢��һ���ӵ�����ǰ��һ�������еĵ��˹���
/// </summary>
public class PeashooterBehaviour : Plant
{
    [Header("������ӵ���Ԫ������")]
    [SerializeField]
    private Elements element = Elements.None;
    /// <summary>
    /// ������ӵ���Ԫ������
    /// </summary>
    public Elements Element { get => element; set => element = value; }

    [Header("������ӵ���Ԥ���������")]
    [SerializeField]
    private string bulletName = "PhysicalBullet";
    /// <summary>
    /// ������ӵ�������
    /// </summary>
    public string BulletName { get => bulletName; set => bulletName = value; }


    [Header("�����ӵ��ļ�������룩")]
    [SerializeField]
    private int timeDistance = 2000;
    /// <summary>
    /// ������ʱ�䣨���룩
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
    /// �Ƿ��й����ڹ���������
    /// </summary>
    /// <returns></returns>
    protected bool HaveMonster()
    {
        Area area = new FrontLine();
        Vector2Int nowPos = GameController.Instance.WorldToGrid(transform.position);
        return GameController.Instance.MonstersController.HaveMonster(area, nowPos);
    }
    /// <summary>
    /// �����ӵ�
    /// </summary>
    protected void ReleasePeaBullet()
    {
        //��ȡ�ӵ�����ͼ��Ϣ
        PeaBulletData data = FlyerPrefabSerializer.Instance.GetFlyerData<PeaBulletData>(bulletName);
        //��ȡ�ӵ����ϵĽű�
        PeaBulletBehaviour flyer = GameController.Instance.FlyersController.AddFlyer<PeaBulletBehaviour>(data, transform.position);
        flyer.AvailableArea = new FrontLine();//�ӵ��ķ�ΧΪǰһ��  
        flyer.ElementType = element;//�ı��ӵ���Ԫ���˺�
        flyer.AtkDmg = Data.AtkPower;//�㶹���˺��빥���ߵĹ�������ͬ
        flyer.CanAddElement = true;//�㶹һֱ���Ը���Ԫ��

    }
}
