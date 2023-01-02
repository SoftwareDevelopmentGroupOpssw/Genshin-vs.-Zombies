using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 魔物控制器
/// </summary>
public class MonstersController
{
    public static readonly GameObject MonsterFatherObject = new GameObject("Monsters");

    private List<Monster> monsters = new List<Monster>();
    /// <summary>
    /// 场上怪物的数量
    /// </summary>
    public int MonsterCount => monsters.Count;
    /// <summary>
    /// 在指定的像素位置添加魔物
    /// </summary>
    /// <param name="data">魔物数据</param>
    /// <param name="worldPos">世界位置</param>
    /// <returns>魔物对象</returns>
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
    /// 移除魔物
    /// </summary>
    /// <param name="monster">魔物对象</param>
    public void RemoveMonster(Monster monster)
    {
        monsters.Remove(monster);
        monster.Data.Dispose();
    }
    /// <summary>
    /// 当前区域是否有怪物
    /// </summary>
    /// <param name="area">区域</param>
    /// <param name="startPos">观测点（根据观测点的不同，Area的实际范围也不同）</param>
    /// <returns>结果</returns>
    public bool HaveMonster(Area area, Vector2Int startPos)
    {
        ILevelData level = GameController.Instance.LevelData;
        foreach(Monster monster in monsters)//区域中所有的怪物
        {
            Vector2Int pos = GameController.Instance.WorldToGrid(monster.transform.position);
            foreach(Vector2Int grid in area.GetArea(level,startPos))//区域中所有的格子
            {
                if (pos == grid)
                    return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 找到一个在最前的Monster
    /// </summary>
    /// <returns></returns>
    public Monster GetMostForwardMonster()
    {
        if(monsters.Count > 0)
        {
            Monster monster = monsters[0];
            for(int i = 1; i < monsters.Count; i++)
            {
                if (monsters[i].transform.position.x < monster.transform.position.x) //新找到的魔物比之前找到的更前
                    monster = monsters[i];
            }
            return monster;
        }
        return null;
    }
    /// <summary>
    /// 遍历所有的魔物
    /// </summary>
    /// <param name="action">遍历的函数</param>
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
