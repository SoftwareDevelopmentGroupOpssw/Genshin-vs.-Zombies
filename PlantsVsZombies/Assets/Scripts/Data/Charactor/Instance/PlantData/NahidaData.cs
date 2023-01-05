using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 纳西妲数据
/// </summary>
public class NahidaData : PlantData
{
    public NahidaData(GameObject original,Sprite cardSprite):base (original,cardSprite)
    {

    }
    public override int EnergyCost => 225;

    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 6;

    public override int CoolTime => 12000;

    public override string PlantName => "Nahida";

    public override string Description => "纳西妲：能隔一段时间向前方扔一个草刺，对一行的敌人造成草元素伤害。";
}
