using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÄÉÎ÷æ§Êı¾İ
/// </summary>
public class NahidaData : PlantData
{
    public NahidaData(GameObject original,Sprite cardSprite):base (original,cardSprite)
    {

    }
    public override int EnergyCost => 300;

    public override int Health { get; set; } = 100;
    public override int AtkPower { get; set; } = 6;

    public override int CoolTime => 12000;

    public override string PlantName => "Nahida";
}
