using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ��ͨ��ʬ���ж��ű�
/// </summary>
public class CommonZombie : Monster, IDamageable
{
    private CommonMonsterHandler handler;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid;
    private Collider2D collider;
    private Animator animator;
    private bool isAlive = true;

    public GameObject FallingHead;
    public override IEffectHandler Handler => handler;



    private void Start()
    {
        handler = new CommonMonsterHandler(Data);

        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();

        //�������һ����·����
        System.Random walkStyle = new System.Random();
        animator.SetInteger("WalkStyle", walkStyle.Next(1, 3));

    }



    /// <summary>
    /// ��·
    /// </summary>
    public void Walk()
    {
        rigid.velocity = Vector2.left * Data.Speed / 100;
        animator.speed = Data.Speed / 50f;
    }
    /// <summary>
    /// �����ܵ�Ԫ�ظ���ʱ����Ч(�ı���ɫ)
    /// </summary>
    public void RefreshElementState()
    {
        Color GetColor(Elements element)
        {
            switch (element)
            {
                case Elements.Water:
                    return new Color(0, 0.5f, 1f);
                case Elements.Fire:
                    return new Color(1f, 0.25f, 0);
                case Elements.Ice:
                    return new Color(0, 1f, 1f);
                case Elements.Electric:
                    return new Color(0.75f, 0, 1f);
                //����Ҳ��ḽ��
                case Elements.Grass:
                    return Color.green;
            }
            return Color.black;
        }
        Elements[] elements = Data.GetAllElements();
        Color color;
        if (elements.Length == 0)//û��Ԫ�ظ��ţ��ָ���������ɫ
            color = Color.white;
        else if (elements.Length == 1)//ֻ��һ��Ԫ�ظ���
            color = GetColor(elements[0]);
        else//�ж��Ԫ�ظ���
        {
            color = GetColor(elements[0]);
            for(int i = 1; i < elements.Length; i++)
            {
                color = (color + GetColor(elements[i])) / 2;
            }
        }
        sprite.color = color;
    }
    /// <summary>
    /// ������ͼɾ��
    /// </summary>
    public void CheckLocation()
    {
        if (GameController.Instance.WorldToGrid(transform.position) == new Vector2(-1, -1))
            Destroy(gameObject);
    }

    string str = "";
    public void Update()
    {
        handler.CheckEffect(Data.GetEffects());
        if (isAlive)
        {
            Walk();
            CheckLocation();

            RefreshElementState();
        }
        if (Data.Health <= 0 && isAlive)
            StartCoroutine(Die());
        if(Data.ToString() != str)
        {
            str = Data.ToString();
            //Debug.Log(str);
        }
    }
    /// <summary>
    /// ����ʱ�������߼�
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        isAlive = false;
        rigid.velocity = Vector2.zero;
        collider.enabled = false;//�ر���ײ�У������Ͳ����赲�ӵ�
        float secondsBeforeBodyDisappear = 2;
        animator.Play("Die");
        FallingHead.SetActive(true);
        FallingHead.GetComponent<SpriteRenderer>().color = sprite.color;//�ѵ���ͷ���óɺ��Լ�һ������ɫ
        yield return new WaitForSecondsRealtime(secondsBeforeBodyDisappear);
        Destroy(gameObject);
    }

    public IDamageReceiver GetReceiver()
    {
        return Data;
    }
}
