using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 刷新模块
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
    /// 生成新的魔物
    /// </summary>
    public void GenerateNewMonster()
    {
        ILevelData level = GameController.Instance.LevelData;

        System.Random random = new System.Random();
        int row = random.Next(1, level.Row);//随机选一行

        Vector3 worldPos = GameController.Instance.GridToWorld(new Vector2Int(level.Col,row), GridPosition.Right);
        controller.AddMonster(monsters.Dequeue(), worldPos);
    }
    /// <summary>
    /// 帧更新时调用，尝试生成魔物
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
    /// 随机生成多个魔物
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    IEnumerator GenerateCoroutine(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GenerateNewMonster();//帧更新结束时尝试生成魔物
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
