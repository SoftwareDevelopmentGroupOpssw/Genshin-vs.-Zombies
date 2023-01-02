using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ·�Ͻ�ʬ��·�ϲ�������
/// (·�ϵ���������ʧЧ��ʹ����ͨ��ʬ������)
/// </summary>
public class RoadConeZombieData : MonsterData
{
    public RoadConeZombieData(GameObject original) : base(original)
    {
        strength = 120;
        health = 370;
        speed = 16;
        atkPower = 4;

        //ȫԪ�ؿ���0.3
        SetResistance(0.3f, Elements.None);
        SetResistance(0.3f, Elements.Water);
        SetResistance(0.3f, Elements.Fire);
        SetResistance(0.3f, Elements.Ice);
        SetResistance(0.3f, Elements.Electric);
        SetResistance(0.3f, Elements.Wind);
        SetResistance(0.3f, Elements.Stone);
        SetResistance(0.3f, Elements.Grass);
    }
}
