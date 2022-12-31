using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mona : PeashooterBehaviour
{
    private const int ATK_COOL_TIME = 3000;
    private Animator animator;//¶¯»­×´Ì¬»ú

    protected override Elements Element => Elements.Water;

    protected override IFlyerData Bullet => FlyerSerializer.Instance.GetFlyerData("MonaWaterBullet");
    protected override int TimeDistance => 2000;

    // Start is called before the first frame update
    protected void Start()
    { 
        animator = GetComponent<Animator>();
    }
    void Atk()
    {
        animator.SetTrigger("Atk");//´¥·¢¹¥»÷¶¯»­
        Debug.Log("ÄªÄÈ¹¥»÷");
    }
    // Update is called once per frame
    protected override void CustomUpdate()
    {
        Atk();
    }
    void OnDestroy()
    {
        
    }
}
