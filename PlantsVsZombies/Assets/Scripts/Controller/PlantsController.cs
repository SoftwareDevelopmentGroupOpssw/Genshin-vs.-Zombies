using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ֲ�������
/// </summary>
public class PlantsController
{
    private ILevelData levelData;
    public PlantsController(ILevelData level)
    {
        this.levelData = level;
    }
    /// <summary>
    /// ���ֲ��
    /// </summary>
    /// <param name="data">ֲ����Ϣ</param>
    /// <param name="gridPos">ֲ����ӵĸ���λ��</param>
    /// <returns>ֲ�����</returns>
    public Plant AddPlant(IPlantData data,Vector2Int gridPos)
    {
        //TODO:�ø����������ڸ������괦���һ��ֲ����� ����������
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// ��ֲ��ӵ�ͼ��ɾ��
    /// </summary>
    /// <param name="plant">ɾ����ֲ�����</param>
    public void RemovePlant(Plant plant)
    {
        //TODO:ɾ��ֲ�����
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Ѱ��ֲ��
    /// </summary>
    /// <param name="gridPos">Ѱ�ҵĸ�������</param>
    /// <returns>���д��ڸ����ϵ�ֲ��</returns>
    public Plant[] SearchPlant(Vector2Int gridPos)
    {
        //TODO:��ָ������������Ѱ�����е�ֲ��
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Ѱ�ҷ�Χ�����е�ֲ��
    /// </summary>
    /// <param name="area">Ѱ�ҵķ�Χ</param>
    /// <returns>Ѱ�ҵ������е�ֲ��</returns>
    public Plant[] SearchPlant(Area area)
    {
        //TODO:��ָ����Χ����Ѱ�����е�ֲ��
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// �������е�ֲ��
    /// </summary>
    /// <param name="action">�����ĺ���</param>
    public void Foreach(UnityAction<Plant> action)
    {
        //TODO����������
    }
}
