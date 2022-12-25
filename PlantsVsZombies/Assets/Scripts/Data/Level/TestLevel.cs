using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// For only testing.
/// </summary>
public class TestLevel : ILevelData
{
    private int row = 5;
    private int col = 10;
    private Sprite sprite;
    private List<IMonsterData> monsterList = new List<IMonsterData>();

    public TestLevel(Sprite sprite)
    {
        this.sprite = sprite;
    }
    public int Row => row;

    public int Col => col;

    public Sprite Sprite => sprite;

    public Queue<IMonsterData> MonsterList => new Queue<IMonsterData>();

    public Vector2Int GridToPixel(Vector2Int gridPos, GridPosition pos)
    {
        return new Vector2Int(960, 540);
    }

    public Vector2Int PixelToGrid(Vector2Int pixelPos)
    {
        return new Vector2Int(0, 0);
    }
}
