using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 砂糖数据
/// </summary>
public class SucroseData : PlantData
{
    public SucroseData(GameObject original, Sprite cardSprite) : base(original, cardSprite)
    {

    }
    public override int EnergyCost => 100;
    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 20;

    public override int CoolTime => 7500;

    public override string PlantName => "Sucrose";

    public override string Description => "砂糖：能隔一段时间向前方扔一个风眼，对接触到的第一个魔物造成风元素伤害。";
}

