using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 角色接口
/// </summary>
public interface ICharactorData:IGameobjectData
{
    /// <summary>
    /// 角色当前生命值
    /// </summary>
    public int Health { get; set; }
    /// <summary>
    /// 角色攻击力
    /// </summary>
    public int AtkPower { get; set; }
}
