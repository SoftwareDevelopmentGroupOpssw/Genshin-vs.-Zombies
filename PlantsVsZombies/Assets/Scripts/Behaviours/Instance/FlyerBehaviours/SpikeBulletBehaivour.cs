using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����һ���ķ�����Ľű������һ�е�ħ����Թᴩ
/// </summary>
public class SpikeBulletBehaivour : Bullet
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Collider2D colliders;
    private ILevelData level => GameController.Instance.LevelData;
    /// <summary>
    /// ����õ��Ŀ�����
    /// </summary>
    private Vector2Int[] area;

    [Header("����˺�")]
    [SerializeField]
    private int damage;

    [Header("����˶��ٶ�")]
    [SerializeField]
    private int velocity = 5;
    public int Velocity { get => velocity; set => velocity = value; }

    [Header("��̵�Ԫ������")]
    [SerializeField]
    private Elements element;

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
        if (area == null)
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

    void Update()
    {
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
            target.GetReceiver().ReceiveDamage(bulletDamage);
        }
    }
}
