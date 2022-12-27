using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// testmonster是一只普通的魔物
/// </summary>
public class TestMonsterData : MonsterData
{
    public TestMonsterData()
    {
        strength = 100;
        health = 100;
        speed = 50;
        atkPower = 100;
        effectHandler = new CommonMonsterHandler(this);//使用普通魔物 效果处理器来处理魔物身上的效果
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
        return $"Stength:{Strength},Health:{Health},Speed:{Speed},AttackPower{AtkPower},Resistance:{GetResistance(Elements.None)}" + elementStr  ;
    }

    protected override void RealAction()
    {
        Debug.Log("Monster is moving");
    }
}
