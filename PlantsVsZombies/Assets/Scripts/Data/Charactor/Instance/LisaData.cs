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

    public override int EnergyCost => 150;

    public override int Health { get; set; } = 100;
    public override int AtkPower { get; set; } = 0;

    public override int CoolTime => 7000;

    public override string PlantName => "Lisa";
}
