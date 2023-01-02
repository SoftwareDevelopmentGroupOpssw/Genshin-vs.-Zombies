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
    float nowTime = 0;
    Coroutine coroutine;
    IEnumerator CountCoroutine()
    {
        nowTime = 0;
        available = false;
        while(nowTime < miliseconds / 1000)
        {
            yield return 1;
            nowTime += Time.unscaledDeltaTime;

            if(available == true)//���ֶ���ͣ
            {
                coroutine = null;
                yield break;
            }
        }
        available = true;
        coroutine = null;
        OnComplete?.Invoke();
    }
    /// <summary>
    /// ��ʼ����ʱ
    /// </summary>
    public void StartCountDown()
    {
        if (coroutine == null)
            coroutine = GameController.Instance.StartCoroutine(CountCoroutine());//��GameController��Э��
    }
    /// <summary>
    /// ֹͣ��ʱ������
    /// </summary>
    public void Reset()
    {
        if (coroutine != null)
        {

            available = true;
        }
    }
}
