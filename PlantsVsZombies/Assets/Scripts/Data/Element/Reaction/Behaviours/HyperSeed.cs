using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ų�����׷�ٵ��Ľű�
/// </summary>
public class HyperSeed : MonoBehaviour, IElementalDamage
{
    public int AtkDmg { get; set; } = Bloom.HyperBloomDamage;
    public Elements ElementType { get; set; } = Elements.Grass;
    public bool CanAddElement { get; set; } = false;

    private bool isTriggered = false;//��û�д�����Ч��
    Coroutine nowCoroutine;//��������ִ�е�Э��

    void Start()
    {
        nowCoroutine = StartCoroutine(ExplodeCoroutine());
    }

    /// <summary>
    /// ��ը�����ɹ��̵�Coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator ExplodeCoroutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.right * HyperExplosion.ExplodeRadius + transform.position;
        float explodeSpeed = 1.5f;
        for (float i = 0; i < 1; i += Time.deltaTime * explodeSpeed)
        {
            transform.position = Vector3.Lerp(startPos, endPos, i);//��ֵ
            yield return 1;
        }
        transform.position = endPos;
        nowCoroutine = StartCoroutine(TrackingCoroutine());
    }
    /// <summary>
    /// ׷�ٹ����Coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator TrackingCoroutine()
    {
        Monster monster = GameController.Instance.MonstersController.GetMostForwardMonster();
        if(monster != null)
        {
            Vector3 startPos = transform.position;
            Vector3 endPos;
            try
            {
                endPos = monster.transform.position + new Vector3(Random.value * 0.5f,Random.value * 0.5f); //��Ŀ��λ�ý����Ŷ�
            }
            catch (MissingReferenceException)
            {
                nowCoroutine = StartCoroutine(TrackingCoroutine());//����׷��
                yield break;
            }
            transform.right = (endPos - startPos).normalized;//�泯��Ŀ���
            float trackSpeed = 2;
            for (float i = 0; i < 1; i += Time.deltaTime * trackSpeed)
            {
                transform.position = Vector3.Lerp(startPos, endPos, i);
                yield return 1;
            }
            if (!isTriggered)//�����յ㻹û�д���
            {
                nowCoroutine = StartCoroutine(TrackingCoroutine());//����׷��
                yield break;
            }
        }
        else//��������û�д��Ĺ���ʱ
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && target is Monster )
        {
            isTriggered = true;
            GetComponent<Collider2D>().enabled = false;//ȡ����ײ�У���ֹͬʱ�Զ��Ŀ�괥��
            StopCoroutine(nowCoroutine);//ֹͣ��ը��׷��Э��
            target.GetReceiver().ReceiveDamage(this);//�����ܵ��˺�
            Destroy(gameObject);//�ݻ�
        }
    }
}
