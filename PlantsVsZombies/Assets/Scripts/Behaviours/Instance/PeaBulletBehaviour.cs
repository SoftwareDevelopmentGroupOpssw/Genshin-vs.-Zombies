using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 像豌豆子弹一样的飞行物：向右直线飞行，超过区域删除，只能对第一个目标造成效果
/// </summary>
public class PeaBulletBehaviour : Flyer, IElementalDamage
{
    private ILevelData level => GameController.Instance.LevelData;
    private Vector3 levelPos => GameController.Instance.Level.transform.position;
    private Vector2Int[] area;
    /// <summary>
    /// 是否砸到怪物身上触发了伤害
    /// </summary>
    private bool isTriggered = false;

    [Header("豌豆伤害")]
    [SerializeField]
    private int damage;
    public int AtkDmg { get => damage; set => damage = value; }
    
    [Header("豌豆运动速度")]
    [SerializeField]
    private int velocity = 5;
    public int Velocity { get => velocity; set => velocity = value; }

    [Header("豌豆的元素类型")]
    [SerializeField]
    private Elements element;
    public Elements ElementType { get => element; set => element = value; }

    [Header("豌豆打击触发时切换的图片")]
    [SerializeField]
    private Sprite brokenSprite;


    public bool CanAddElement { get; set; }



    void Start()
    {
        //计算目前能打到的位置
        area = AvailableArea.GetArea(level, level.WorldToGrid(transform.position, levelPos));
        GetComponent<Rigidbody2D>().velocity = Vector2.right * Velocity;
    }
    
    /// <summary>
    /// 子弹飞行
    /// </summary>
    void Moving()
    {
        bool isInArea = false;
        foreach (var grid in area)
        {
            if (level.WorldToGrid(transform.position, levelPos) == grid)
            {
                isInArea = true; break;
            }
        }
        if (!isInArea)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator BrokenCoroutine()
    {
        float waitSecondsBeforeDestroy = 0.2f;//在Destroy前显示broken的时间
        GetComponent<SpriteRenderer>().sprite = brokenSprite;
        yield return new WaitForSecondsRealtime(waitSecondsBeforeDestroy);
        Destroy(gameObject);
    }
    void Update()
    {
        if (!isTriggered)
            Moving();
    }
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Monster" && !isTriggered) //没有触发过，而且砸到了一个怪物的身上
        {
            Monster monster = collision.gameObject.GetComponent<Monster>();
            if(monster is IDamageable)
            {
                isTriggered = true;
                StartCoroutine(BrokenCoroutine());
                (monster as IDamageable).GetReceiver().ReceiveDamage(this);
            }
        }
    }
}
