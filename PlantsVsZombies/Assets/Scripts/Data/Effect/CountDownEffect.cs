using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// ʱЧЧ���Ļ���
/// </summary>
public abstract class CountDownEffect: IEffect
{
    private CountDown countDown;
    protected CountDown CountDown
    {
        get
        {
            if (countDown == null)
            {
                countDown = new CountDown(MilisecondsDuration);
                countDown.OnComplete += End;
            }
            return countDown;
        }
    }
    
    protected CountDownEffect()
    {
        State = EffectState.Initialized;
    }
    
    /// <summary>
    /// Ч���ĳ���ʱ��
    /// </summary>
    public abstract int MilisecondsDuration { get; }
    
    /// <summary>
    /// Ч��������
    /// </summary>
    public abstract string EffectName { get; }

    /// <summary>
    /// Ч��������״̬
    /// </summary>
    public EffectState State { get; protected set; }

    /// <summary>
    /// ��ʼ����ʱ
    /// </summary>
    protected void Start()
    {
        CountDown.StartCountDown();
        State = EffectState.Processing;
    }
    /// <summary>
    /// ����Ч����StateתΪEnd
    /// </summary>
    protected void End()
    {
        if(State != EffectState.Error)//���������ܹ�����
            State = EffectState.End;
        CountDown.OnComplete -= End;
    }
    /// <summary>
    /// ��;������ʶΪ����
    /// </summary>
    protected void Error()
    {
        State = EffectState.Error;
    }
    /// <summary>
    /// ������д����������Ч��ʱ����
    /// </summary>
    /// <param name="target"></param>
    public virtual void EnableEffect(IGameobjectData target) { }
    /// <summary>
    /// ������д�������Ƴ�Ч��ʱ����
    /// </summary>
    /// <param name="target"></param>
    public virtual void DisableEffect(IGameobjectData target) { }
    /// <summary>
    /// ������д����������Ч��ʱ����
    /// </summary>
    /// <param name="target"></param>
    public virtual void UpdateEffect(IGameobjectData target) { }
    public abstract IGameobjectData Caster { get; }
}
