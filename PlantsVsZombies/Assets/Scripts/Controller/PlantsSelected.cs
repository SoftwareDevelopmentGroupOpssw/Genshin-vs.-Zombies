using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ѡ���ֲ��
/// </summary>
public class PlantsSelected
{
    /// <summary>
    /// ֲ������
    /// </summary>
    public IPlantData Data { get; }
    /// <summary>
    /// ���ֲ���Ƿ���Է��ã���ȴʱ�䵽��û�У�
    /// </summary>
    public bool IsReady { get; }
    /// <summary>
    /// ��ʾ�ڿ������Sprite
    /// </summary>
    public Sprite SelectSprite { get; }
}
