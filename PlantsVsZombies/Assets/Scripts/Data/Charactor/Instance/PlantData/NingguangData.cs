using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NingguangData : PlantData
{
    public NingguangData(GameObject original, Sprite cardSprite) : base(original, cardSprite)
    {
    }

    public override int EnergyCost => 300;

    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 80;

    public override int CoolTime => 20000;

    public override string PlantName => "Ningguang";
}
