using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������ű�����
/// </summary>
public abstract class Flyer : BaseGameobject
{
    // Start is called before the first frame update
    /// <summary>
    /// �������ܹ����������
    /// </summary>
    public Area AvailableArea { get; set; }
    public IFlyerData Data { get; set; }
}
