using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格子地图基类：所有的格子大小相同，矩形均匀分布
/// </summary>
public abstract class GridLevel : ILevelData
{
    /// <summary>
    /// 使用偏移量（格子的Left,Right）来获取像素位置时，和格子真正的左边框、右边框的偏移量
    /// （并不是正好落在左边框和右边框处）
    /// </summary>
    private float offsetPercent = 0.1f;

    /// <summary>
    /// 格子中最左上角的像素位置
    /// 以Sprite的最左上角像素为（0,0）
    /// </summary>
    protected abstract Vector2Int GridsLeftTopCornorPos { get; }

    /// <summary>
    /// 格子像素宽度
    /// </summary>
    protected abstract int GridWidth { get; }

    /// <summary>
    /// 格子像素高度
    /// </summary>
    protected abstract int GridHeight { get; }

    public abstract int Row { get; }

    public abstract int Col { get; }

    public abstract Sprite Sprite { get; }
    public abstract IMonsterData[] MonsterTypes { get; }

    public abstract IEnumerator GenerateEnumerator();

    public Vector3 GridToWorld(Vector2Int gridPos, GridPosition pos, Vector3 levelPos)
    {
        if (gridPos.x > Col || gridPos.x < 0 || gridPos.y > Row || gridPos.y < 0)
            throw new System.IndexOutOfRangeException($"The grid position is out side of the map. The map size:({Row},{Col}); The grid position:({gridPos.x},{gridPos.y}). ");
        else
        {
            float pixels = Sprite.pixelsPerUnit;


            //得到格子左上角的像素位置
            int pixelX = GridsLeftTopCornorPos.x + (gridPos.x - 1) * GridWidth;
            int pixelY = GridsLeftTopCornorPos.y + (gridPos.y - 1) * GridHeight;


            pixelY += GridHeight / 2;//将像素位置调到居中
            switch (pos)
            {
                case GridPosition.Left:
                    pixelX += System.Convert.ToInt32 (offsetPercent * GridWidth);
                    break;
                case GridPosition.Middle:
                    pixelX += GridWidth / 2;
                    break;
                case GridPosition.Right:
                    pixelX += System.Convert.ToInt32( (1 -offsetPercent) * GridWidth);
                    break;

            }
            return levelPos + new Vector3(pixelX/pixels, -pixelY/pixels, 0);//格子坐标和世界坐标轴的y轴方向相反
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPos, Vector3 levelPos)
    {
        Vector3 deltaPos = worldPos - levelPos;
        float pixels = Sprite.pixelsPerUnit;
        int pixelX = System.Convert.ToInt32(deltaPos.x * pixels);
        int pixelY = System.Convert.ToInt32(-deltaPos.y * pixels);
 
        Vector2Int deltaPixel = new Vector2Int(pixelX, pixelY) - GridsLeftTopCornorPos;

        if(deltaPixel.x < 0 || deltaPixel.y < 0)//超过边界
            return new Vector2Int(-1, -1);
        
        
        int gridX = deltaPixel.x / GridWidth + 1;
        int gridY = deltaPixel.y / GridHeight + 1;
        
        if (gridX > Col || gridY > Row)//超过边界
            return new Vector2Int(-1, -1);
        
        return new Vector2Int(gridX, gridY);
    }

}
