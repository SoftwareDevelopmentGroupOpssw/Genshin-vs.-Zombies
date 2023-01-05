using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������
/// </summary>
public class NahidaData : PlantData
{
    public NahidaData(GameObject original,Sprite cardSprite):base (original,cardSprite)
    {

    }
    public override int EnergyCost => 225;

    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 6;

    public override int CoolTime => 12000;

    public override string PlantName => "Nahida";

    public override string Description => "����槣��ܸ�һ��ʱ����ǰ����һ���ݴ̣���һ�еĵ�����ɲ�Ԫ���˺���";
}
