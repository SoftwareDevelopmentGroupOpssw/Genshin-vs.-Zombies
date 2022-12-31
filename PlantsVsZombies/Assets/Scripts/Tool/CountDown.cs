using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 计时器类（沙漏)
/// 会被Gamecontroller暂停计时
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
    /// 改变值会影响下次倒计时时间
    /// </summary>
    public int MilisecondsCountDown { get => miliseconds; set => miliseconds = value; }
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
    Coroutine coroutine;
    IEnumerator CountCoroutine()
    {
        available = false;
        yield return new WaitForSecondsRealtime(MilisecondsCountDown / 1000f);
        available = true;
        OnComplete?.Invoke();
        coroutine = null;
    }
    public void StartCountDown()
    {
        if (coroutine == null)
            coroutine = MonoManager.Instance.StartCoroutine(CountCoroutine());
    }
}
