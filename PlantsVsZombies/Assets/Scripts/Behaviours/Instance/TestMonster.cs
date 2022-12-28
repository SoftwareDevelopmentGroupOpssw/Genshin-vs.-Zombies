using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Testmonster���ж��ű�
/// </summary>
public class TestMonster : Monster
{
    private CommonMonsterHandler handler;
    public TestMonster()
    {
        handler = new CommonMonsterHandler(Data as IMonsterData);
    }
    public override IEffectHandler Handler => handler;
    public void Update()
    {
        handler.CheckEffect(Data.GetEffects());
    }
}
