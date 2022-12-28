using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 像豌豆子弹一样的飞行物：向右直线飞行，超过区域删除，只能对第一个目标造成效果
/// </summary>
public abstract class PeaBulletBehaviour : Flyer
{
    private ILevelData level => GameController.Instance.LevelData;
    private Vector3 levelPos => GameController.Instance.Level.transform.position;
    private Vector2Int[] area;
    protected abstract int Velocity { get; }
    protected virtual void Start()
    {
        //计算目前能打到的位置
        area = AvailableArea.GetArea(level, level.WorldToGrid(transform.position, levelPos));
        GetComponent<Rigidbody2D>().velocity = Vector2.right * Velocity;
    }
    protected virtual void Update()
    {
        bool isInArea = false;
        foreach(var grid in area)
        {
            if(level.WorldToGrid(transform.position,levelPos) == grid)
            {
                isInArea = true;break;
            }
        }
        if (!isInArea)
        {
            Destroy(gameObject);
        }
    }
}
