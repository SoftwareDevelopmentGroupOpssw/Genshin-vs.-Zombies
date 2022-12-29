using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ÀöÉ¯
/// </summary>
public class Lisa : PeashooterBehaviour
{
    private Animator animator;//¶¯»­×´Ì¬»ú
    // Start is called before the first frame update

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// ¹¥»÷¶¯×÷
    /// </summary>
    void Atk()
    {
        animator.SetTrigger("Atk");//´¥·¢¹¥»÷¶¯»­
    }

    private void Update()
    {
        if (CountDown.Available && HaveMonster())
        {
            Atk();
            CountDown.StartCountDown();
        }
    }
}
