using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 可以被魔物攻击的接口
/// </summary>
public interface IMonsterAttackable
{
    public ICharactorData GetData();
}
