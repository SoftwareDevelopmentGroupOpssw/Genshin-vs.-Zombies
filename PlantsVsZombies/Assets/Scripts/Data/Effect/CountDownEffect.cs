using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ʱЧЧ���Ļ���
/// </summary>
public abstract class CountDownEffect: IEffect
{
    protected CountDown countDown;

    protected CountDownEffect()
    {
        State = EffectState.Initialized;
    }
    
    /// <summary>
    /// Ч���ĳ���ʱ��
    /// </summary>
    public abstract int MilisecondsDuration { get; }

    public abstract string EffectName { get; }

    public EffectState State { get; private set; }

    /// <summary>
    /// ��ʼ����ʱ
    /// </summary>
    protected void Start()
    {
        if (countDown == null)
        {
            countDown = new CountDown(MilisecondsDuration);
            countDown.OnComplete += End;
        }
        countDown.StartCountDown();
        State = EffectState.Processing;
    }
    /// <summary>
    /// ��������ʱ
    /// </summary>
    protected void End()
    {
        State = EffectState.End;
    }

    public abstract IGameobjectData Caster { get; }
}
