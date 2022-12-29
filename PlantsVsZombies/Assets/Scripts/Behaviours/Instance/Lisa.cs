using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lisa : PeashooterBehaviour
{
    private Animator animator;//����״̬��
    // Start is called before the first frame update

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// ��������
    /// </summary>
    void Atk()
    {
        animator.SetTrigger("Atk");//������������
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