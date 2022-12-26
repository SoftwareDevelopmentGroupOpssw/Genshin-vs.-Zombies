using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ټӳ�Ч��
/// </summary>
public class SpeedEffect : CountDownEffect
{
    /// <summary>
    /// Ч����ɵ����ٸı�ֵ
    /// </summary>
    private int deltaSpeed;

    private float percent;
    /// <summary>
    /// �ƶ�Ч���İٷֱ�
    /// </summary>
    public float Percent => percent;

    public override string EffectName => "SpeedEffect";

    private int duration;
    public override int MilisecondsDuration { get => duration; }

    private IGameobjectData caster;
    public override IGameobjectData Caster => caster;

    /// <summary>
    /// ��������Ч��
    /// </summary>
    /// <param name="caster">ʩ����</param>
    /// <param name="percent">����Ч�������ٷֱȣ�����Ϊ��ֵҲ����Ϊ��ֵ</param>
    /// <param name="miliSecondsDuration">����ʱ�䣨���룩</param>
    public SpeedEffect(IGameobjectData caster, float percent,int miliSecondsDuration)
    {
        this.percent = percent;
        this.caster = caster;
        duration = miliSecondsDuration;
    }
    /// <summary>
    /// ����/��������
    /// </summary>
    /// <param name="speed">�ı��������ֵ����</param>
    public void EnableEffect(ref int speed)
    {
        deltaSpeed = System.Convert.ToInt32(speed * percent);
        speed += deltaSpeed;
        Start();
    }
    /// <summary>
    /// ���ı������ֵ����
    /// </summary>
    /// <param name="speed">�ı��������ֵ������</param>
    public void DisableEffect(ref int speed)
    {
        speed -= deltaSpeed;
    }
}
