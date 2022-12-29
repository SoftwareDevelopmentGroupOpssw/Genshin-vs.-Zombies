using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YanfeiData : PlantData
{
    public YanfeiData(GameObject original, Sprite cardSprite):base(original,cardSprite)
    {
    
    }


    public override int EnergyCost => 200;

    public override int Health { get; set; } = 100;
    public override int AtkPower { get; set; } = 0;


    public override int CoolTime => 10000;

    public override string PlantName => "Yanfei";

}
