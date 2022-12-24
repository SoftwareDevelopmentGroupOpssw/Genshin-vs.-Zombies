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
    /// 游戏开始后到现在的毫秒数
    /// </summary>
    public int milisecondsSinceGameStarted
    {
        get
        {
            return (DateTime.Now - startTime).Milliseconds;
        }
    }
    /// <summary>
    /// 生成新的魔物
    /// </summary>
    public void GenerateNewMonsters()
    {
        //TODO:生成新的魔物
    }
    /// <summary>
    /// 帧更新时调用
    /// </summary>
    public void Update()
    {
        GenerateNewMonsters();//帧更新结束时尝试生成魔物
    }
}
