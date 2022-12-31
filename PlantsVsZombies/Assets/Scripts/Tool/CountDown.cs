using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ��ʱ���ࣨɳ©)
/// �ᱻGamecontroller��ͣ��ʱ
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
    /// �ı�ֵ��Ӱ���´ε���ʱʱ��
    /// </summary>
    public int MilisecondsCountDown { get => miliseconds; set => miliseconds = value; }
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
