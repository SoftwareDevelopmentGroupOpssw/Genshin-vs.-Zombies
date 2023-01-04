using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÄªÄÈ½Å±¾
/// </summary>
public class Mona : PeashooterBehaviour
{
    private Animator animator;//¶¯»­×´Ì¬»ú
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// ¹¥»÷¶¯×÷
    /// </summary>
    void Atk()
    {
        animator.SetTrigger("Atk");//´¥·¢¹¥»÷¶¯»­
    }

    protected override void Update()
    {
        base.Update();
        if (CountDown.Available && HaveMonster())
        {
            Atk();
            CountDown.StartCountDown();
        }
    }
}
