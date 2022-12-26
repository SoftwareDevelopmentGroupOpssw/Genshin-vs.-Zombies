using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 计时器类（沙漏)
/// </summary>
public class CountDown
{
    private int miliseconds;
    private bool available;
    /// <summary>
    /// 是否正在停止计时状态中
    /// </summary>
    public bool Available => miliseconds == -1?true:available;
    /// <summary>
    /// 计时器计时时间
    /// </summary>
    public int MilisecondsCountDown => miliseconds;
    /// <summary>
    /// 当计时器每次归零时调用
    /// </summary>
    public event System.Action OnComplete;
    /// <summary>
    /// 以指定毫秒间隔启动计时器，传入-1则不会开始计时
    /// </summary>
    /// <param name="milisecondsCountDown">毫秒间隔</param>
    public CountDown(int milisecondsCountDown)
    {
        miliseconds = milisecondsCountDown;
        available = true;
    }
    private Task task;
    private void ThreadFunc()
    {
        available = false;
        Thread.Sleep(miliseconds);
        available = true;
        OnComplete?.Invoke();
        task = null;
    }
    public void StartCountDown()
    {
        if(task == null && miliseconds != -1)
        {
            task = Task.Run(ThreadFunc);
        }
    }
}
