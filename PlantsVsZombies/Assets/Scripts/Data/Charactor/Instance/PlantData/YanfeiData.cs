using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YanfeiData : PlantData
{
    public YanfeiData(GameObject original, Sprite cardSprite):base(original,cardSprite)
    {
    
    }


    public override int EnergyCost => 175;

    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 25;


    public override int CoolTime => 7500;

    public override string PlantName => "Yanfei";

}
