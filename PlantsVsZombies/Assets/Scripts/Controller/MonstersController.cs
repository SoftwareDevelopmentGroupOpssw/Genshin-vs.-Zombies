using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ħ�������
/// </summary>
public class MonstersController
{
    private ILevelData levelData;
    public MonstersController(ILevelData level)
    {
        this.levelData = level;
    }
    /// <summary>
    /// ��ָ��������λ�����ħ��
    /// </summary>
    /// <param name="data">ħ������</param>
    /// <param name="pixelPos">����λ��</param>
    /// <returns>ħ�����</returns>
    public Monster AddMonster(IMonsterData data, Vector2Int pixelPos)
    {
        //TODO:���ħ�ﲢ����
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// �Ƴ�ħ��
    /// </summary>
    /// <param name="monster">ħ�����</param>
    public void RemoveMonster(Monster monster)
    {
        //TODO:�Ƴ�ħ��
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// ��ѯָ��������������ǰ����ħ��
    /// </summary>
    /// <param name="gridPos">��������</param>
    /// <returns>��ǰ����ħ�����</returns>
    public Monster SearchFirstMonster(Vector2Int gridPos)
    {
        //TODO:��ѯ����ħ��
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// ��ѯָ�����������е�ħ��
    /// </summary>
    /// <param name="gridPos">���Ӷ���</param>
    /// <returns>ħ���������</returns>
    public Monster[] SearchMonsters(Vector2Int gridPos)
    {
        //TODO:��ѯħ��
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// ����ָ����Χ�е�ħ�ﲢ����
    /// �������ȼ�Ϊ����һ���ȼ�ǰ�����󷽣��ڶ����ȼ��Ϸ����·�
    /// </summary>
    /// <param name="area"></param>
    /// <returns>ָ����Χ�е�ħ�����������</returns>
    public Monster[] SearchMonsters(Area area)
    {
        //TODO:��ѯָ�������е�ħ�ﲢ�������򼯺�
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// �������е�ħ��
    /// </summary>
    /// <param name="action">�����ĺ���</param>
    public void Foreach(UnityAction<Monster> action)
    {
        //TODO����������
    }
}
