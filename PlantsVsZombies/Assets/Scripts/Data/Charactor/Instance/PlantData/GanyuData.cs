using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GanyuData : PlantData
{
    public GanyuData(GameObject original, Sprite cardSprite) : base(original, cardSprite)
    {
    }

    public override int EnergyCost => 125;

    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 25;

    public override int CoolTime => 7500;

    public override string PlantName => "Ganyu";

    public override string Description => "���꣺�ܸ�һ��ʱ����ǰ����һ�����򣬶ԽӴ����ĵ�һ��ħ����ɱ�Ԫ���˺���";
}
