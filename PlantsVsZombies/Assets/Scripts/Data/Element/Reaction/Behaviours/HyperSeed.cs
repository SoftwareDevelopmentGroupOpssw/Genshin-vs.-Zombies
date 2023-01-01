using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 超绽放产生的追踪弹的脚本
/// </summary>
public class HyperSeed : MonoBehaviour, IElementalDamage
{
    public int AtkDmg { get; set; } = Bloom.HyperBloomDamage;
    public Elements ElementType { get; set; } = Elements.Grass;
    public bool CanAddElement { get; set; } = false;

    private bool isTriggered = false;//有没有触发过效果
    Coroutine nowCoroutine;//现在正在执行的协程

    void Start()
    {
        nowCoroutine = StartCoroutine(ExplodeCoroutine());
    }

    /// <summary>
    /// 爆炸被弹飞过程的Coroutine
    /// </summary>
    /// <returns></returns>
    IEnumerator ExplodeCoroutine()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = transform.right * HyperExplosion.ExplodeRadius + transform.position;
        float explodeSpeed = 1.5f;
        for (float i = 0; i < 1; i += Time.deltaTime * explodeSpeed)
        {
            transform.position = Vector3.Lerp(startPos, endPos, i);//插值
            yield return 1;
        }
        transform.position = endPos;
        nowCoroutine = StartCoroutine(TrackingCoroutine());
    }
    /// <summary>
    /// 追踪怪物的Coroutine
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
                endPos = monster.transform.position + new Vector3(Random.value * 0.5f,Random.value * 0.5f); //对目标位置进行扰动
            }
            catch (MissingReferenceException)
            {
                nowCoroutine = StartCoroutine(TrackingCoroutine());//重新追踪
                yield break;
            }
            transform.right = (endPos - startPos).normalized;//面朝着目标点
            float trackSpeed = 2;
            for (float i = 0; i < 1; i += Time.deltaTime * trackSpeed)
            {
                transform.position = Vector3.Lerp(startPos, endPos, i);
                yield return 1;
            }
            if (!isTriggered)//到了终点还没有触发
            {
                nowCoroutine = StartCoroutine(TrackingCoroutine());//重新追踪
                yield break;
            }
        }
        else//当场景上没有存活的怪物时
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
            GetComponent<Collider2D>().enabled = false;//取消碰撞盒，防止同时对多个目标触发
            StopCoroutine(nowCoroutine);//停止爆炸、追踪协程
            target.GetReceiver().ReceiveDamage(this);//让其受到伤害
            Destroy(gameObject);//摧毁
        }
    }
}
