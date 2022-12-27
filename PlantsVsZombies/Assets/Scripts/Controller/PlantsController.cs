using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ֲ�������
/// </summary>
public class PlantsController
{
    private GameObject PlantsFatherObject = new GameObject("Plants");
    private ILevelData levelData;
    private Plant[,] plants;
    public PlantsController(ILevelData level)
    {
        this.levelData = level;
        plants = new Plant[levelData.Row, levelData.Col];
    }
    /// <summary>
    /// ǿ�����ֲ��
    /// </summary>
    /// <param name="data">ֲ����Ϣ</param>
    /// <param name="gridPos">ֲ����ӵĸ���λ��</param>
    /// <returns>ֲ�����</returns>
    public Plant AddPlant(IPlantData data,Vector2Int gridPos)
    {
        GameObject newObj = GameObject.Instantiate(data.OriginalReference,PlantsFatherObject.transform);
        Plant plant = newObj.GetComponent<Plant>();
        newObj.transform.position = levelData.GridToWorld(gridPos, GridPosition.Middle, GameController.Instance.Level.transform.position);
        data.GameObject = newObj;
        return plant;
    }
    public bool TryAddPlant(IPlantData data,Vector2Int gridPos, out Plant plant)
    {
        GameObject newObj = GameObject.Instantiate(data.OriginalReference,PlantsFatherObject.transform);
        plant = newObj.GetComponent<Plant>();
        newObj.transform.position = levelData.GridToWorld(gridPos, GridPosition.Middle, GameController.Instance.Level.transform.position);
        data.GameObject = newObj;
        return true;
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
