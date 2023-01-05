using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ͱ��ʬ����Ͱ��������
/// (��Ͱ����������ʧЧ��ʹ����ͨ��ʬ������)
/// </summary>
public class BucketHeadZombieData : MonsterData
{
    public BucketHeadZombieData(GameObject original) : base(original)
    {
        strength = 150;
        health = 1100;
        speed = 16;
        atkPower = 4;

        //ȫԪ�ؿ���0.3�����2��Ԫ�ؿ��Ժܸߣ��ﵽ0.6
        SetResistance(0.3f, Elements.None);
        SetResistance(0.3f, Elements.Water);
        SetResistance(0.3f, Elements.Fire);
        SetResistance(0.3f, Elements.Ice);
        SetResistance(0.3f, Elements.Electric);
        SetResistance(0.3f, Elements.Wind);
        SetResistance(0.3f, Elements.Stone);
        SetResistance(0.3f, Elements.Grass);

        System.Random r = new System.Random();
        SetResistance(0.6f, (Elements)r.Next(0, 8));
        SetResistance(0.6f, (Elements)r.Next(0, 8));
    }

}
