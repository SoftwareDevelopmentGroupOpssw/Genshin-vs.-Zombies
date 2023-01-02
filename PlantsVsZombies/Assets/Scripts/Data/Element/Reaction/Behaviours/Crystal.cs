using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 结晶碎片效果
/// </summary>
public class Crystal : MonoBehaviour
{
    private bool isTriggered;
    private Rigidbody2D rigid;
    
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(GameController.Instance.WorldToGrid(transform.position) == new Vector2(-1, -1))
        {
            Crystallize.RemoveCrystal(this);
        }
    }
    private void OnEnable()
    {
        isTriggered = false;
    }
    public void StartTracking()
    {
        int trackingVelocity = 8;
        rigid.velocity = Vector2.left * trackingVelocity ;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Plant plant = collision.GetComponent<Plant>();
        
        if(plant != null && !isTriggered)
        {
            isTriggered = true;
            rigid.velocity = Vector2.zero;
            EntitiesController controller = GameController.Instance.EntitiesController;
            GameObject shieldObjInstance = controller.Get(Crystallize.bufferName, Crystallize.ShieldPrefab);
            shieldObjInstance.transform.position = collision.gameObject.transform.position;
            ShieldEffect effect = new ShieldEffect
                (
                    Crystallize.SHEILD_POWER,
                    Crystallize.SHEILD_MILISECONDS_DURATION,
                    SystemObject.Instance,
                    () =>
                    {
                        controller.Put(Crystallize.bufferName,Crystallize.ShieldPrefab,shieldObjInstance);//护盾结束后显示移除
                    }
                 );
            plant.Data?.AddEffect(effect);
            Crystallize.RemoveCrystal(this);
        }
    }
}
