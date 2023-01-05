using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidenShogunData : PlantData
{
    public RaidenShogunData(GameObject original, Sprite cardSprite) : base(original, cardSprite)
    {
    }

    public override int EnergyCost => 50;

    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 0;

    public override int CoolTime => 7500;
    
    public override string PlantName => "RaidenShogun";

    public EnergyType ProduceType => EnergyType.Big;

    public int ProduceDistance => 22000;

    public override string Description => "雷电将军：隔一段时间能够产出能量。";
}
