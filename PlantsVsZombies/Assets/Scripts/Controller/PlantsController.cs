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
    private List<Plant>[,] plants;
    public PlantsController(ILevelData level)
    {
        this.levelData = level;
        plants = new List<Plant>[levelData.Col, levelData.Row];//x��������������y������������
        for(int i = 0; i< levelData.Col; i++)
        {
            for(int j = 0;j < levelData.Row; j++)
            {
                plants[i, j] = new List<Plant>(5);
            }
        }
    }
    /// <summary>
    /// ǿ�����ֲ��
    /// </summary>
    /// <param name="data">ֲ����Ϣ</param>
    /// <param name="gridPos">ֲ����ӵĸ���λ��</param>
    /// <returns>���ý��</returns>
    public bool AddPlant(ref IPlantData data,Vector2Int gridPos)
    {
        GameObject newObj = GameObject.Instantiate(data.OriginalReference,PlantsFatherObject.transform);
        Plant plant = newObj.GetComponent<Plant>();
        newObj.transform.position = levelData.GridToWorld(gridPos, GridPosition.Middle, GameController.Instance.Level.transform.position);
        
        data.GameObject = newObj;
        plant.Data = data;

        plants[gridPos.x - 1, gridPos.y - 1].Add(plant);
        return true;
    }
    /// <summary>
    /// ��ָ���ĸ������Ƴ������õ�ֲ��
    /// </summary>
    /// <param name="gridPos"></param>
    public void RemoveOnePlant(Vector2Int gridPos)
    {
        gridPos -= Vector2Int.one;
        if (plants[gridPos.x, gridPos.y].Count > 0)
        {
            int count = plants[gridPos.x, gridPos.y].Count;
            Plant plant = plants[gridPos.x, gridPos.y][count - 1];//��ȡ���һ��
            
            plant.Data.GameObject = null;
            GameObject.Destroy(plant.gameObject);
        }
    }

    /// <summary>
    /// ����ֲ�����ǿ�ƽ�ָ����ֲ��ӵ�ͼ��ɾ��
    /// </summary>
    /// <param name="plant">ɾ����ֲ�����</param>
    public void RemovePlant(Plant plant)
    {
        foreach(var item in plants)
        {
            if (!item.Remove(plant))//�Ƴ�ʧ��
                continue;
            else //�Ƴ��ɹ�
            {
                plant.Data.GameObject = null;
                GameObject.Destroy(plant);
                return;
            }
        }
    }
    /// <summary>
    /// Ѱ��ֲ��
    /// </summary>
    /// <param name="gridPos">Ѱ�ҵĸ�������</param>
    /// <returns>���д��ڸ����ϵ�ֲ��</returns>
    public Plant[] SearchPlant(Vector2Int gridPos)
    {
        return plants[gridPos.x, gridPos.y].ToArray();
    }
    /// <summary>
    /// Ѱ�ҷ�Χ�����е�ֲ��
    /// </summary>
    /// <param name="area">Ѱ�ҵķ�Χ</param>
    /// <returns>Ѱ�ҵ������е�ֲ��</returns>
    public Plant[] SearchPlant(Area area)
    {
        List<Plant> found = new List<Plant>();
        //Vector2Int[] gridPoss = area.GetArea()
        return null;
    }
    /// <summary>
    /// �������е�ֲ��
    /// </summary>
    /// <param name="action">�����ĺ���</param>
    public void Foreach(UnityAction<Plant> action)
    {

        foreach (var plantList in plants)
        {
            foreach (Plant p in plantList)
            {
                action.Invoke(p);
            }
        }
    }
}
