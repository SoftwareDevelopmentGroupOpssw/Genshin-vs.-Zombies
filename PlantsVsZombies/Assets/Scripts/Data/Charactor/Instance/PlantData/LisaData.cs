using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// РіЩЏЪ§Он
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
}
