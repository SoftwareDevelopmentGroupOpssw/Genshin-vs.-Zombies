using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 像西瓜一样的子弹：可以造成溅射伤害
/// </summary>
public class WatermelonBulletBehaviour : Bullet
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Collider2D colliders;
    private ILevelData level => GameController.Instance.LevelData;
    /// <summary>
    /// 计算得到的可行域
    /// </summary>
    private Vector2Int[] area;


    [Header("西瓜溅射伤害")]
    [SerializeField]
    private int sputterDmg;
    public int SputterDmg { get => sputterDmg; set => sputterDmg = value; }

    [Header("西瓜溅射半径")]
    [SerializeField]
    private float radius;
    public float Radius { get => radius; set => radius = value; }

    [Header("西瓜运动速度")]
    [SerializeField]
    private int velocity = 4;
    public int Velocity { get => velocity; set => velocity = value; }

    [Header("西瓜打击触发时碎掉的图片")]
    [SerializeField]
    private Sprite brokenSprite;

    [Header("西瓜飞行时的的图片")]
    [SerializeField]
    private Sprite flyingSprite;

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
        if (colliders.enabled == true)//还没有被触发过
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
            IDamageReceiver targetReceiver = target.GetReceiver();
            if (targetReceiver != null && targetReceiver.ReceiveDamage(bulletDamage))
            {
                StartCoroutine(BrokenCoroutine());//在碎掉的样子停留一会儿

                //溅射伤害
                Collider2D[] colliders = Physics2D.OverlapCircleAll(collision.gameObject.transform.position, radius);
                foreach(var collider in colliders)
                {
                    IDamageable canbeDamaged = collider.GetComponent<IDamageable>();
                    if (canbeDamaged != null && !(target is Plant))
                    {
                        IDamageReceiver receiver = canbeDamaged.GetReceiver();
                        if(receiver != null)
                            receiver.ReceiveDamage(new SputterDamage() { Damage = sputterDmg });
                    }
                }
            }
        }
    }

    class SputterDamage : IElementalDamage
    {
        public int Damage { get; set; } = 0;
        public Elements ElementType { get; set; } = Elements.None;
        public bool CanAddElement { get; set; } = false;
    }
}
