using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 普通僵尸数据
/// </summary>
public class CommonZombieData : MonsterData
{
    public CommonZombieData(GameObject original) : base(original)
    {
        strength = 60;
        health = 270;
        speed = 16;
        atkPower = 4;

        //全元素抗性0.1
        SetResistance(0.1f, Elements.None);
        SetResistance(0.1f, Elements.Water);
        SetResistance(0.1f, Elements.Fire);
        SetResistance(0.1f, Elements.Ice);
        SetResistance(0.1f, Elements.Electric);
        SetResistance(0.1f, Elements.Wind);
        SetResistance(0.1f, Elements.Stone);
        SetResistance(0.1f, Elements.Grass);
    }
}
