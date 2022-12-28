using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���㶹�ӵ�һ���ķ��������ֱ�߷��У���������ɾ����ֻ�ܶԵ�һ��Ŀ�����Ч��
/// </summary>
public abstract class PeaBulletBehaviour : Flyer
{
    private ILevelData level => GameController.Instance.LevelData;
    private Vector3 levelPos => GameController.Instance.Level.transform.position;
    private Vector2Int[] area;
    protected abstract int Velocity { get; }
    protected virtual void Start()
    {
        //����Ŀǰ�ܴ򵽵�λ��
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
