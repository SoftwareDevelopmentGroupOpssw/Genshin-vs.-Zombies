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
    
    /// <summary>
    /// 效果的名字
    /// </summary>
    public abstract string EffectName { get; }

    /// <summary>
    /// 效果所处的状态
    /// </summary>
    public EffectState State { get; protected set; }

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
    /// 结束效果，State转为End
    /// </summary>
    protected void End()
    {
        if(State != EffectState.Error)//不出错，才能够结束
            State = EffectState.End;
    }
    /// <summary>
    /// 中途出错，标识为出错
    /// </summary>
    protected void Error()
    {
        State = EffectState.Error;
    }
    /// <summary>
    /// 子类重写函数：启动效果时调用
    /// </summary>
    /// <param name="target"></param>
    public virtual void EnableEffect(IGameobjectData target) { }
    /// <summary>
    /// 子类重写函数：移除效果时调用
    /// </summary>
    /// <param name="target"></param>
    public virtual void DisableEffect(IGameobjectData target) { }
    /// <summary>
    /// 子类重写函数：更新效果时调用
    /// </summary>
    /// <param name="target"></param>
    public virtual void UpdateEffect(IGameobjectData target) { }
    public abstract IGameobjectData Caster { get; }
}
