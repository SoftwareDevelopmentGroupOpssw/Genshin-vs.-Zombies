using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������һ�����ӵ���������ɽ����˺�
/// </summary>
public class WatermelonBulletBehaviour : Bullet
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Collider2D colliders;
    private ILevelData level => GameController.Instance.LevelData;
    /// <summary>
    /// ����õ��Ŀ�����
    /// </summary>
    private Vector2Int[] area;


    [Header("���Ͻ����˺�")]
    [SerializeField]
    private int sputterDmg;
    public int SputterDmg { get => sputterDmg; set => sputterDmg = value; }

    [Header("���Ͻ���뾶")]
    [SerializeField]
    private float radius;
    public float Radius { get => radius; set => radius = value; }

    [Header("�����˶��ٶ�")]
    [SerializeField]
    private int velocity = 4;
    public int Velocity { get => velocity; set => velocity = value; }

    [Header("���ϴ������ʱ�����ͼƬ")]
    [SerializeField]
    private Sprite brokenSprite;

    [Header("���Ϸ���ʱ�ĵ�ͼƬ")]
    [SerializeField]
    private Sprite flyingSprite;

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
        if (colliders.enabled == true)//��û�б�������
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
            IDamageReceiver targetReceiver = target.GetReceiver();
            if (targetReceiver != null && targetReceiver.ReceiveDamage(bulletDamage))
            {
                StartCoroutine(BrokenCoroutine());//�����������ͣ��һ���

                //�����˺�
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
