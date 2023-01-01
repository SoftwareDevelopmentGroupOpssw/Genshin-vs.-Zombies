using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 像豌豆子弹一样的飞行物的控制脚本：向右直线飞行，超过区域删除，只能对第一个目标造成效果
/// 豌豆子弹可以造成元素伤害
/// </summary>
public class PeaBulletBehaviour : Bullet
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Collider2D colliders;
    private ILevelData level => GameController.Instance.LevelData;
    /// <summary>
    /// 计算得到的可行域
    /// </summary>
    private Vector2Int[] area;

    [Header("豌豆伤害")]
    [SerializeField]
    private int damage;
    
    [Header("豌豆运动速度")]
    [SerializeField]
    private int velocity = 5;
    public int Velocity { get => velocity; set => velocity = value; }

    [Header("豌豆的元素类型")]
    [SerializeField]
    private Elements element;

    [Header("豌豆打击触发时碎掉的图片")]
    [SerializeField]
    private Sprite brokenSprite;

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
        if(area == null)
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

    IEnumerator BrokenCoroutine()
    {
        colliders.enabled = false; //解除触发器检测
        spriteRenderer.sprite = brokenSprite;
        rigid.velocity = Vector2.zero;//停止移动

        float waitSecondsBeforeDestroy = 0.1f;//在Destroy前显示broken的时间
        yield return new WaitForSecondsRealtime(waitSecondsBeforeDestroy);
        RemoveThis();
    }
    void Update()
    {
        if(colliders.enabled == true)//还没有被触发过
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
            if(target.GetReceiver().ReceiveDamage(bulletDamage))
                StartCoroutine(BrokenCoroutine());//在碎掉的样子停留一会儿
        }
    }
}
