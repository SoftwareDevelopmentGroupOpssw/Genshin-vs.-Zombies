using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 铁桶僵尸的铁桶部分数据
/// (铁桶掉落后此数据失效，使用普通僵尸的数据)
/// </summary>
public class BucketHeadZombieData : MonsterData
{
    public BucketHeadZombieData(GameObject original) : base(original)
    {
        strength = 150;
        health = 1100;
        speed = 16;
        atkPower = 4;

        //全元素抗性0.3，随机2种元素抗性很高，达到0.6
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
