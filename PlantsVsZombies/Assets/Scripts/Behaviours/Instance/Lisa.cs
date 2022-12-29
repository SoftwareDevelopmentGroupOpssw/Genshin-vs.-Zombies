using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lisa : PeashooterBehaviour
{
    private Animator animator;//动画状态机
    // Start is called before the first frame update

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// 攻击动作
    /// </summary>
    void Atk()
    {
        animator.SetTrigger("Atk");//触发攻击动画
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
