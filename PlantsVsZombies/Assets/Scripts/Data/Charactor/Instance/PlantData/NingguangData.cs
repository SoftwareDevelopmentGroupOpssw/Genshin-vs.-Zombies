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

    public override int CoolTime => 12000;

    public override string PlantName => "Ningguang";

    public override string Description => "���⣺�ܸ�һ��ʱ����ǰ����һ����ʯ����ʯ�ڽӴ���һ������ʱ�����ѣ��Ե�һ����������������˺���������Χ�ĵ�����ɽ����˺���";
}
