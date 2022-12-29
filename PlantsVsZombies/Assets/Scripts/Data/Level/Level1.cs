using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level1
/// </summary>
public class Level1 : Level
{
    private int row = 5;
    private int col = 10;
    private Sprite sprite;
    private List<string> monsterList = new List<string>();

    public Level1(Sprite sprite)
    {
        this.sprite = sprite;
        for(int i = 0; i < 100; i++)
        {
            monsterList.Add("CommonZombie");
        }
    }
    public override int Row => row;

    public override int Col => col;

    public override Sprite Sprite => sprite;

    public override Queue<IMonsterData> MonsterList
    {
        get
        {
            Queue<IMonsterData> monsters = new Queue<IMonsterData>(monsterList.Count);
            foreach(var monsterName in monsterList)
            {
                monsters.Enqueue(MonsterPrefabSerializer.Instance.GetMonsterData(monsterName));
            }
            return monsters;
        }
    }

    protected override Vector2Int GridsLeftTopCornorPos => new Vector2Int(128, 128);

    protected override int GridWidth => 137;

    protected override int GridHeight => 134;
}
