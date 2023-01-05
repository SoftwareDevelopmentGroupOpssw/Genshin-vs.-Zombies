using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ɰ������
/// </summary>
public class SucroseData : PlantData
{
    public SucroseData(GameObject original, Sprite cardSprite) : base(original, cardSprite)
    {

    }
    public override int EnergyCost => 100;
    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 20;

    public override int CoolTime => 7500;

    public override string PlantName => "Sucrose";

    public override string Description => "ɰ�ǣ��ܸ�һ��ʱ����ǰ����һ�����ۣ��ԽӴ����ĵ�һ��ħ����ɷ�Ԫ���˺���";
}

