using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBuilder:Area
{
    /// <summary>
    /// ����һ��Area
    /// </summary>
    /// <param name="area">Area����</param>
    public void AddArea(Area area)
    {
        //TODO:���һ��Area��Builder��ȥ
    }
    /// <summary>
    /// ��ȡ���builder������area���Ĳ���
    /// </summary>
    /// <param name="level">�ؿ�����</param>
    /// <param name="gridPos">�������</param>
    /// <returns>�����뷶Χ�ĸ�㼯��</returns>
    public override Vector2Int[] GetArea(ILevelData level, Vector2Int gridPos)
    {
        //TODO�������е�Area�����ĸ��ȡ�����������س�ȥ
        throw new System.NotImplementedException();
    }
}
