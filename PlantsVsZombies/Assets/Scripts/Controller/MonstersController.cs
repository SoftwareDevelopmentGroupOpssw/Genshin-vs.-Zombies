using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ħ�������
/// </summary>
public class MonstersController
{
    public static readonly GameObject MonsterFatherObject = new GameObject("Monsters");

    private List<Monster> monsters = new List<Monster>();
    /// <summary>
    /// ���Ϲ��������
    /// </summary>
    public int MonsterCount => monsters.Count;
    /// <summary>
    /// ��ָ��������λ�����ħ��
    /// </summary>
    /// <param name="data">ħ������</param>
    /// <param name="worldPos">����λ��</param>
    /// <returns>ħ�����</returns>
    public Monster AddMonster(IMonsterData data, Vector3 worldPos)
    {
        GameObject monster = GameObject.Instantiate(data.OriginalReference,MonsterFatherObject.transform);
        monster.transform.position = worldPos;
        data.GameObject = monster;

        Monster component = monster.GetComponent<Monster>();
        component.Data = data;

        monsters.Add(component);
        return component;
    }
    /// <summary>
    /// �Ƴ�ħ��
    /// </summary>
    /// <param name="monster">ħ�����</param>
    public void RemoveMonster(Monster monster)
    {
        monsters.Remove(monster);
        monster.Data.Dispose();
    }
    /// <summary>
    /// ��ǰ�����Ƿ��й���
    /// </summary>
    /// <param name="area">����</param>
    /// <param name="startPos">�۲�㣨���ݹ۲��Ĳ�ͬ��Area��ʵ�ʷ�ΧҲ��ͬ��</param>
    /// <returns>���</returns>
    public bool HaveMonster(Area area, Vector2Int startPos)
    {
        ILevelData level = GameController.Instance.LevelData;
        foreach(Monster monster in monsters)//���������еĹ���
        {
            Vector2Int pos = GameController.Instance.WorldToGrid(monster.transform.position);
            foreach(Vector2Int grid in area.GetArea(level,startPos))//���������еĸ���
            {
                if (pos == grid)
                    return true;
            }
        }
        return false;
    }
    /// <summary>
    /// �ҵ�һ������ǰ��Monster
    /// </summary>
    /// <returns></returns>
    public Monster GetMostForwardMonster()
    {
        if(monsters.Count > 0)
        {
            Monster monster = monsters[0];
            for(int i = 1; i < monsters.Count; i++)
            {
                if (monsters[i].transform.position.x < monster.transform.position.x) //���ҵ���ħ���֮ǰ�ҵ��ĸ�ǰ
                    monster = monsters[i];
            }
            return monster;
        }
        return null;
    }
    /// <summary>
    /// �������е�ħ��
    /// </summary>
    /// <param name="action">�����ĺ���</param>
    public void Foreach(UnityAction<Monster> action)
    {
        monsters.ForEach((monster) => action.Invoke(monster));
    }
    public void Clear()
    {
        foreach(var monster in monsters)
        {
            monster.Data.GameObject = null;
            GameObject.Destroy(monster.gameObject);
        }
        monsters.Clear();
    }
}
