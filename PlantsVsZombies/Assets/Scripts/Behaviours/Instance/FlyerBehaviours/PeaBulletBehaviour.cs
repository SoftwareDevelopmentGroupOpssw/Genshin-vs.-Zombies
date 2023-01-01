using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���㶹�ӵ�һ���ķ�����Ŀ��ƽű�������ֱ�߷��У���������ɾ����ֻ�ܶԵ�һ��Ŀ�����Ч��
/// �㶹�ӵ��������Ԫ���˺�
/// </summary>
public class PeaBulletBehaviour : Bullet
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Collider2D colliders;
    private ILevelData level => GameController.Instance.LevelData;
    /// <summary>
    /// ����õ��Ŀ�����
    /// </summary>
    private Vector2Int[] area;

    [Header("�㶹�˺�")]
    [SerializeField]
    private int damage;
    
    [Header("�㶹�˶��ٶ�")]
    [SerializeField]
    private int velocity = 5;
    public int Velocity { get => velocity; set => velocity = value; }

    [Header("�㶹��Ԫ������")]
    [SerializeField]
    private Elements element;

    [Header("�㶹�������ʱ�����ͼƬ")]
    [SerializeField]
    private Sprite brokenSprite;

    [Header("�㶹����ʱ�ĵ�ͼƬ")]
    [SerializeField]
    private Sprite flyingSprite;

    protected override BulletDamage bulletDamage => new BulletDamage() { AtkDmg = damage, ElementType = element, CanAddElement = true };

    /// <summary>
    /// �Ƴ��Լ�
    /// </summary>
    void RemoveThis()
    {
        area = null;//����Ѿ�����õ��������´γ���ʱ���¼���
        GameController.Instance.FlyersController.RemoveFlyer(this);
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        colliders = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable()//ÿ�α����������������ʱ���ý��г�ʼ��
    {
        //��ʼ����
        colliders.enabled = true;//������ײ�нű�
        rigid.velocity = Vector2.right * Velocity;//�����ٶ�
        spriteRenderer.sprite = flyingSprite;//����Ϊ����ͼƬ
    }


    /// <summary>
    /// ����ӵ��Ƿ��������ڣ�����ֱ��ɾ��
    /// </summary>
    void CheckLocation()
    {
        if(area == null)
            //����Ŀǰ�ܴ򵽵�λ��
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
            RemoveThis();//�Ƴ��Լ�
        }
    }

    IEnumerator BrokenCoroutine()
    {
        colliders.enabled = false; //������������
        spriteRenderer.sprite = brokenSprite;
        rigid.velocity = Vector2.zero;//ֹͣ�ƶ�

        float waitSecondsBeforeDestroy = 0.1f;//��Destroyǰ��ʾbroken��ʱ��
        yield return new WaitForSecondsRealtime(waitSecondsBeforeDestroy);
        RemoveThis();
    }
    void Update()
    {
        if(colliders.enabled == true)//��û�б�������
            CheckLocation();
    }
   
    /// <summary>
    /// ��ײ�����
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.gameObject.GetComponent<IDamageable>();//�Ӵ���Ŀ��������һ��IDamageable�Ľű�
        if (target != null && !(target is Plant))//Ŀ�겻����һ��ֲ��
        {
            if(target.GetReceiver().ReceiveDamage(bulletDamage))
                StartCoroutine(BrokenCoroutine());//�����������ͣ��һ���
        }
    }
}
