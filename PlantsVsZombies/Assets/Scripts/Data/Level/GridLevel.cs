using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ӵ�ͼ���ࣺ���еĸ��Ӵ�С��ͬ�����ξ��ȷֲ�
/// </summary>
public abstract class GridLevel : ILevelData
{
    /// <summary>
    /// ʹ��ƫ���������ӵ�Left,Right������ȡ����λ��ʱ���͸�����������߿��ұ߿��ƫ����
    /// ������������������߿���ұ߿򴦣�
    /// </summary>
    private float offsetPercent = 0.1f;

    /// <summary>
    /// �����������Ͻǵ�����λ��
    /// ��Sprite�������Ͻ�����Ϊ��0,0��
    /// </summary>
    protected abstract Vector2Int GridsLeftTopCornorPos { get; }

    /// <summary>
    /// �������ؿ��
    /// </summary>
    protected abstract int GridWidth { get; }

    /// <summary>
    /// �������ظ߶�
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


            //�õ��������Ͻǵ�����λ��
            int pixelX = GridsLeftTopCornorPos.x + (gridPos.x - 1) * GridWidth;
            int pixelY = GridsLeftTopCornorPos.y + (gridPos.y - 1) * GridHeight;


            pixelY += GridHeight / 2;//������λ�õ�������
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
            return levelPos + new Vector3(pixelX/pixels, -pixelY/pixels, 0);//��������������������y�᷽���෴
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPos, Vector3 levelPos)
    {
        Vector3 deltaPos = worldPos - levelPos;
        float pixels = Sprite.pixelsPerUnit;
        int pixelX = System.Convert.ToInt32(deltaPos.x * pixels);
        int pixelY = System.Convert.ToInt32(-deltaPos.y * pixels);
 
        Vector2Int deltaPixel = new Vector2Int(pixelX, pixelY) - GridsLeftTopCornorPos;

        if(deltaPixel.x < 0 || deltaPixel.y < 0)//�����߽�
            return new Vector2Int(-1, -1);
        
        
        int gridX = deltaPixel.x / GridWidth + 1;
        int gridY = deltaPixel.y / GridHeight + 1;
        
        if (gridX > Col || gridY > Row)//�����߽�
            return new Vector2Int(-1, -1);
        
        return new Vector2Int(gridX, gridY);
    }

}
