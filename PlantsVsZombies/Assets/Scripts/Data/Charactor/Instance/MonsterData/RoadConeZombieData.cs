using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 路障僵尸的路障部分数据
/// (路障掉落后此数据失效，使用普通僵尸的数据)
/// </summary>
public class RoadConeZombieData : MonsterData
{
    public RoadConeZombieData(GameObject original) : base(original)
    {
        strength = 120;
        health = 370;
        speed = 16;
        atkPower = 4;

        //全元素抗性0.3
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
