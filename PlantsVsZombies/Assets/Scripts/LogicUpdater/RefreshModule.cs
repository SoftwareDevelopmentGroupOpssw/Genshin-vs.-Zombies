using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ˢ��ģ��
/// </summary>
public class RefreshModule
{
    private const int SUMMON_DISTANCE_TIME = 8000;
    private Queue<IMonsterData> monsters;
    private MonstersController controller;
    private CountDown countDown = new CountDown(SUMMON_DISTANCE_TIME);
    public RefreshModule(MonstersController controller,Queue<IMonsterData> data)
    {
        this.controller = controller;
        monsters = data;
    }
    /// <summary>
    /// �����µ�ħ��
    /// </summary>
    public void GenerateNewMonster()
    {
        ILevelData level = GameController.Instance.LevelData;

        System.Random random = new System.Random();
        int row = random.Next(1, level.Row);//���ѡһ��

        Vector3 worldPos = GameController.Instance.GridToWorld(new Vector2Int(level.Col,row), GridPosition.Right);
        controller.AddMonster(monsters.Dequeue(), worldPos);
    }
    /// <summary>
    /// ֡����ʱ���ã���������ħ��
    /// </summary>
    public void Update()
    {
        if (countDown.Available)
        {
            System.Random count = new System.Random();
            int times = count.Next(1, 6);
            MonoManager.Instance.StartCoroutine(GenerateCoroutine(times));
            countDown.StartCountDown();
        }
    }
    /// <summary>
    /// ������ɶ��ħ��
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    IEnumerator GenerateCoroutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GenerateNewMonster();//֡���½���ʱ��������ħ��
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
