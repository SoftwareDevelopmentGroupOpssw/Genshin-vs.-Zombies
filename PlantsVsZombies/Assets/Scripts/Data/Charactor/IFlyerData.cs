using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������ӿ�
/// </summary>
public interface IFlyerData:IGameobjectData
{
    /// <summary>
    /// ���������һ������ʱ�������ĺ���
    /// </summary>
    /// <param name="target">Ŀ������</param>
    public void OnTriggered(GameObject target);
}
