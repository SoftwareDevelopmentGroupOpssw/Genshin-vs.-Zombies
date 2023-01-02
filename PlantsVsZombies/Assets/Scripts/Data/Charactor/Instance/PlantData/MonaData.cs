using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public MonaData(GameObject original,Sprite cardSprite):base(original,cardSprite)
    {
        
    }

    public override int EnergyCost => 100;

    private int health = 300;
    public override int Health { get => health; set => health = value; }

    private int atkPower = 20;
    public override int AtkPower { get => atkPower; set => atkPower = value; }

    public override int CoolTime => 7500;

    public override string PlantName => "Mona";

}
