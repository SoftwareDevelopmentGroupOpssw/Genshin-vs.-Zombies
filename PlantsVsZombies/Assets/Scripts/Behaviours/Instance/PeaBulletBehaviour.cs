using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���㶹�ӵ�һ���ķ��������ֱ�߷��У���������ɾ����ֻ�ܶԵ�һ��Ŀ�����Ч��
/// </summary>
public class PeaBulletBehaviour : Flyer, IElementalDamage
{
    private ILevelData level => GameController.Instance.LevelData;
    private Vector3 levelPos => GameController.Instance.Level.transform.position;
    private Vector2Int[] area;
    /// <summary>
    /// �Ƿ��ҵ��������ϴ������˺�
    /// </summary>
    private bool isTriggered = false;

    [Header("�㶹�˺�")]
    [SerializeField]
    private int damage;
    public int AtkDmg { get => damage; set => damage = value; }
    
    [Header("�㶹�˶��ٶ�")]
    [SerializeField]
    private int velocity = 5;
    public int Velocity { get => velocity; set => velocity = value; }

    [Header("�㶹��Ԫ������")]
    [SerializeField]
    private Elements element;
    public Elements ElementType { get => element; set => element = value; }

    [Header("�㶹�������ʱ�л���ͼƬ")]
    [SerializeField]
    private Sprite brokenSprite;


    public bool CanAddElement { get; set; }



    void Start()
    {
        //����Ŀǰ�ܴ򵽵�λ��
        area = AvailableArea.GetArea(level, level.WorldToGrid(transform.position, levelPos));
        GetComponent<Rigidbody2D>().velocity = Vector2.right * Velocity;
    }
    
    /// <summary>
    /// �ӵ�����
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
        float waitSecondsBeforeDestroy = 0.2f;//��Destroyǰ��ʾbroken��ʱ��
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
        if(collision.gameObject.tag == "Monster" && !isTriggered) //û�д������������ҵ���һ�����������
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
