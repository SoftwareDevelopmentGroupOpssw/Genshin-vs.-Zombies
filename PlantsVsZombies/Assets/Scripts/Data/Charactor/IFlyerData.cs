using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 飞行物接口
/// </summary>
public interface IFlyerData:IGameobjectData
{
    /// <summary>
    /// 当飞行物打到一个物体时，触发的函数
    /// </summary>
    /// <param name="target">目标物体</param>
    public void OnTriggered(GameObject target);
}
