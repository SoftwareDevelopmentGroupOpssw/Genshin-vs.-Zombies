using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshModule
{
    private DateTime startTime;
    public RefreshModule()
    {
        startTime = DateTime.Now;
    }
    /// <summary>
    /// ��Ϸ��ʼ�����ڵĺ�����
    /// </summary>
    public int milisecondsSinceGameStarted
    {
        get
        {
            return (DateTime.Now - startTime).Milliseconds;
        }
    }
    /// <summary>
    /// �����µ�ħ��
    /// </summary>
    public void GenerateNewMonsters()
    {
        //TODO:�����µ�ħ��
    }
    /// <summary>
    /// ֡����ʱ����
    /// </summary>
    public void Update()
    {
        GenerateNewMonsters();//֡���½���ʱ��������ħ��
    }
}
