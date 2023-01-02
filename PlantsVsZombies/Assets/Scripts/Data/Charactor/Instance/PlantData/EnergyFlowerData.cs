using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyFlowerData : PlantData
{
    public EnergyFlowerData(GameObject original, Sprite cardSprite) : base(original, cardSprite)
    {
    }

    public override int EnergyCost => 50;

    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 0;

    public override int CoolTime => 7500;
    
    public override string PlantName => "EnergyFlower";

    public EnergyType ProduceType => EnergyType.Big;

    public int ProduceDistance => 22000;
}
