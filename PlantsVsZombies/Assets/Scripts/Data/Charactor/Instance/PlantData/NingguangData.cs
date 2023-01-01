using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NingguangData : PlantData
{
    public NingguangData(GameObject original, Sprite cardSprite) : base(original, cardSprite)
    {
    }

    public override int EnergyCost => 225;

    public override int Health { get; set; } = 150;
    public override int AtkPower { get; set; } = 10;

    public override int CoolTime => 7500;

    public override string PlantName => "Ningguang";
}
