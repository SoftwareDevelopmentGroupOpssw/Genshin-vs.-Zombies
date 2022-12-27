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
    /// <param name="target">�ı����ٵĶ���</param>
    /// <exception cref="System.NotSupportedException">������һ���ǹ��ﵥλ�������Ч��</exception>
    public override void EnableEffect(IGameobjectData target)
    {
        if (!(target is IMonsterData))//������һ������
        {
            Error();
            throw new System.NotSupportedException("The speed effect can only be added on monsters.");//����ֻ�й����������������Ч��
        }
        else
        {
            IMonsterData monster = target as IMonsterData;
            deltaSpeed = System.Convert.ToInt32(monster.Speed * percent);
            monster.Speed += deltaSpeed;
            Start();
        }
    }
    /// <summary>
    /// ���ı������ֵ����
    /// </summary>
    /// <param name="target">�ı����ٵĶ���</param>
    public override void DisableEffect(IGameobjectData target)
    {
        //�ܱ����,˵��һ���ǹ���
        IMonsterData monster = target as IMonsterData;
        monster.Speed -= deltaSpeed;
    }
}
