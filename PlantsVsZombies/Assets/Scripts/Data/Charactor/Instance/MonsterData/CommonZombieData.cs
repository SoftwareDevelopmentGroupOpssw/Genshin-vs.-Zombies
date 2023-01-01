using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÆÕÍ¨½©Ê¬Êý¾Ý
/// </summary>
public class CommonZombieData : MonsterData
{
    public CommonZombieData(GameObject original) :base(original)
    {
        strength = 130;
        health = 100;
        speed = 35;
        atkPower = 3;
        //SetResistance(0.1f, Elements.None);
        //SetResistance(0.1f, Elements.Water);
        //SetResistance(0.1f, Elements.Fire);
        //SetResistance(0.1f, Elements.Ice);
        //SetResistance(0.1f, Elements.Electric);
        //SetResistance(0.1f, Elements.Wind);
        //SetResistance(0.1f, Elements.Stone);
        //SetResistance(0.1f, Elements.Grass);
    }

    public override string ToString()
    {
        string elementStr = "Elements:";
        Elements[] elements = GetAllElements();
        for(int i = 0;i < elements.Length; i++)
        {
            elementStr += System.Enum.GetName(typeof(Elements),elements[i]) + ",";
        }
        return $"Stength:{Strength},Health:{Health},Speed:{Speed},AttackPower{AtkPower},Resistance:{GetResistance(Elements.None)}" + elementStr  ;
    }
}
