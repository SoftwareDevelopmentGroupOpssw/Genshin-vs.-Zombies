using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNutData : PlantData
{
    public WallNutData(GameObject original, Sprite cardSprite) : base(original, cardSprite)
    {
    }

    public override int EnergyCost => 50;

    public override int Health { get; set; } = 4000;
    public override int AtkPower { get; set; } = 0;
    public override int CoolTime => 30000;

    public override string PlantName => "WallNut";
}
