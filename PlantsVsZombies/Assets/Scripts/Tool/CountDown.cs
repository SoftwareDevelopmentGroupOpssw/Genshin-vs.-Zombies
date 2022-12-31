using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ��ʱ���ࣨɳ©)
/// </summary>
public class CountDown
{
    private int miliseconds;
    private bool available;
    /// <summary>
    /// �Ƿ�����ֹͣ��ʱ״̬��
    /// </summary>
    public bool Available => miliseconds == -1?true:available;
    /// <summary>
    /// ��ʱ����ʱʱ��
    /// </summary>
    public int MilisecondsCountDown => miliseconds;
    /// <summary>
    /// ����ʱ��ÿ�ι���ʱ����
    /// </summary>
    public event System.Action OnComplete;
    /// <summary>
    /// ��ָ��������������ʱ��������-1�򲻻Ὺʼ��ʱ
    /// </summary>
    /// <param name="milisecondsCountDown">������</param>
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
