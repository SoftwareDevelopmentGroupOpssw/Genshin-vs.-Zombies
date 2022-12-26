using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 时效效果的基类
/// </summary>
public abstract class CountDownEffect: IEffect
{
    protected CountDown countDown;

    protected CountDownEffect()
    {
        State = EffectState.Initialized;
    }
    
    /// <summary>
    /// 效果的持续时间
    /// </summary>
    public abstract int MilisecondsDuration { get; }

    public abstract string EffectName { get; }

    public EffectState State { get; private set; }

    /// <summary>
    /// 开始倒计时
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
    /// 结束倒计时
    /// </summary>
    protected void End()
    {
        State = EffectState.End;
    }

    public abstract IGameobjectData Caster { get; }
}
