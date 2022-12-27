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
    private List<IMonsterData> monsterList = new List<IMonsterData>();

    public Level1(Sprite sprite)
    {
        this.sprite = sprite;
    }
    public override int Row => row;

    public override int Col => col;

    public override Sprite Sprite => sprite;

    public override Queue<IMonsterData> MonsterList => new Queue<IMonsterData>();

    protected override Vector2Int GridsLeftTopCornorPos => new Vector2Int(128, 128);

    protected override int GridWidth => 137;

    protected override int GridHeight => 134;
}
