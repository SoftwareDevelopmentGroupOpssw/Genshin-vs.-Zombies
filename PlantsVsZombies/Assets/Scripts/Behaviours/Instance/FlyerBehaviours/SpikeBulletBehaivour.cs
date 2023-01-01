using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 像尖刺一样的飞行物的脚本：打击一行的魔物，可以贯穿
/// </summary>
public class SpikeBulletBehaivour : Bullet
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Collider2D colliders;
    private ILevelData level => GameController.Instance.LevelData;
    /// <summary>
    /// 计算得到的可行域
    /// </summary>
    private Vector2Int[] area;

    [Header("尖刺伤害")]
    [SerializeField]
    private int damage;

    [Header("尖刺运动速度")]
    [SerializeField]
    private int velocity = 5;
    public int Velocity { get => velocity; set => velocity = value; }

    [Header("尖刺的元素类型")]
    [SerializeField]
    private Elements element;

    [Header("豌豆飞行时的的图片")]
    [SerializeField]
    private Sprite flyingSprite;

    protected override BulletDamage bulletDamage => new BulletDamage() { AtkDmg = damage, ElementType = element, CanAddElement = true };

    /// <summary>
    /// 移除自己
    /// </summary>
    void RemoveThis()
    {
        area = null;//清空已经计算得到的区域，下次出现时重新计算
        GameController.Instance.FlyersController.RemoveFlyer(this);
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        colliders = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable()//每次被飞行物控制器激活时调用进行初始化
    {
        //初始化：
        colliders.enabled = true;//启用碰撞盒脚本
        rigid.velocity = Vector2.right * Velocity;//设置速度
        spriteRenderer.sprite = flyingSprite;//设置为飞行图片
    }


    /// <summary>
    /// 检查子弹是否还在区域内，不在直接删除
    /// </summary>
    void CheckLocation()
    {
        if (area == null)
            //计算目前能打到的位置
            area = AvailableArea.GetArea(level, GameController.Instance.WorldToGrid(transform.position));

        bool isInArea = false;
        foreach (var grid in area)
        {
            if (GameController.Instance.WorldToGrid(transform.position) == grid)
            {
                isInArea = true; break;
            }
        }
        if (!isInArea)
        {
            RemoveThis();//移除自己
        }
    }

    void Update()
    {
        CheckLocation();
    }

    /// <summary>
    /// 碰撞器检测
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();//接触的目标身上有一个IDamageable的脚本
        if (target != null && !(target is Plant))//目标不能是一个植物
        {
            target.GetReceiver().ReceiveDamage(bulletDamage);
        }
    }
}
