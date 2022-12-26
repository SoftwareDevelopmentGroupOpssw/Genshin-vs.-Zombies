using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMonsterData : MonsterData
{
    public TestMonsterData()
    {
        strength = 100;
        health = 100;
        speed = 50;
        atkPower = 100;
        SetResistance(0.1f, Elements.Ice);
        SetResistance(0, Elements.Fire);
    }

    public override string ResourcePath => throw new System.NotImplementedException();


    public override IGameobjectData Instantiate()
    {
        return new TestMonsterData();
    }

    public override string ToString()
    {
        string elementStr = "Elements:";
        Elements[] elements = GetAllElements();
        for(int i = 0;i < elements.Length; i++)
        {
            elementStr += System.Enum.GetName(typeof(Elements),elements[i]) + ",";
        }
        return $"Stength:{Strength},Health:{Health},Speed:{Speed},AttackPower{AtkPower}," + elementStr  ;
    }

    protected override void RealAction()
    {
        Debug.Log("Monster is moving");
    }
}
