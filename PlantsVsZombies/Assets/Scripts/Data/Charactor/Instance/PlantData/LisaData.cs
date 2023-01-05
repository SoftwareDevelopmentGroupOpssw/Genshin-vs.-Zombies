using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 丽莎数据
/// </summary>
public class LisaData : PlantData
{
    public LisaData(GameObject original, Sprite cardSprite): base(original, cardSprite)
    {

    }

    public override int EnergyCost => 175;

    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 20;

    public override int CoolTime => 7500;

    public override string PlantName => "Lisa";

    public override string Description => "丽莎：能隔一段时间向前方扔一个雷球，对接触到的第一个魔物造成雷元素伤害。";
}
