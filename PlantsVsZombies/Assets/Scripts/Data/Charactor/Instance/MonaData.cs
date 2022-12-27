using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 莫娜角色数据
/// </summary>
public class MonaData : PlantData
{
    /// <summary>
    /// 由PlantSerializer调用，初始化数据
    /// 不要使用这个方法进行实例化，而应使用PlantSerializer的方法去获得一个新的数据
    /// </summary>
    /// <param name="original">原始对象</param>
    /// <param name="cardSprite">卡牌图片</param>
    public MonaData(GameObject original,Sprite cardSprite)
    {
        this.original = original;
        this.cardSprite = cardSprite;
    }

    private CountDown countDown = new CountDown(2000);
    public override bool isReady => countDown.Available;

    public override int EnergyCost => 100;

    private int health = 100;
    public override int Health { get => health; set => health = value; }

    private int atkPower;
    public override int AtkPower { get => atkPower; set => atkPower = value; }

    private GameObject original;
    public override GameObject OriginalReference { get => original; }

    private Sprite cardSprite;
    public override Sprite CardSprite => cardSprite;

    public override int CoolTime => 5;

    public override void Action()
    {
        Debug.Log("莫娜开始攻击");
    }

    public override void OnAwake()
    {
        Debug.Log("莫娜出现");
    }

    public override void OnDestroy()
    {
        Debug.Log("莫娜死亡");
    }
}
