using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ͼ��Χ����
/// </summary>
public abstract class Area
{
    /// <summary>
    /// ��ȡ�����ͼ�ϣ���ָ���������꣬���еĿ��и������귶Χ
    /// </summary>
    /// <param name="level">�ؿ�����</param>
    /// <param name="gridPos">��������</param>
    /// <returns>���б����뷶Χ�ĸ�������</returns>
    public abstract Vector2Int[] GetArea(ILevelData level, Vector2Int gridPos);
}
