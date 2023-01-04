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
    /// ������ӵ���Ԥ���������
    /// �ڷ����ӵ�ʱ����������ֵ�FlyerPrefabSerializer�в��Ҷ�Ӧ�ӵ�����
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
        float replayPercent = 0.5f;//�����Ž��ȴﵽ��ʱ����һ���ٷֱȾͿ�ʼ���²���
        if (lastSource == null || !lastSource.gameObject.activeSelf || lastSource.time > lastSource.clip.length * replayPercent)//������������ȥ�ˣ�����ֹͣ��
             lastSource = AudioManager.Instance.PlayRandomEffectAudio("chomp1", "chomp2");
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
        CommonFlyerData data = FlyerPrefabSerializer.Instance.GetFlyerData<CommonFlyerData>(bulletName);
        //��ȡ�ӵ����ϵĽű�
        GameController.Instance.FlyersController.AddFlyer<Bullet>(data, transform.position, (bullet)=>
        {
            bullet.AvailableArea = new FrontLine();//�ӵ��ķ�ΧΪǰһ��  
            bullet.ElementType = element;//�ı��ӵ���Ԫ���˺�
            bullet.Damage = Data.AtkPower;//�ӵ����˺��빥���ߵĹ�������ͬ
            bullet.CanAddElement = true;//�ӵ�һֱ���Ը���Ԫ��
        });
        AudioManager.Instance.PlayRandomEffectAudio("throw1", "throw2");
    }
}
