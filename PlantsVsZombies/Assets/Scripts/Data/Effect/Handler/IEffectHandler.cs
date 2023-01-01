using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ч������ӿڣ���һ��Ч��������
/// </summary>
public interface IEffectHandler
{
    /// <summary>
    /// �Զ�������ĳ��Ч��
    /// </summary>
    /// <param name="effect">Ч��</param>
    public void EnableEffect(IEffect effect);
    /// <summary>
    /// �Զ�����Ч������ʱ���и���Ч��
    /// </summary>
    /// <param name="effect">Ч��</param>
    public void UpdateEffect(IEffect effect);
    /// <summary>
    /// �Զ����Ƴ�Ч��
    /// </summary>
    /// <param name="effect">Ч��</param>
    public void DisableEffect(IEffect effect);
    /// <summary>
    /// �Զ����һ��Ч�����з��������á����»����Ƴ���
    /// </summary>
    public void CheckEffect();
    /// <summary>
    /// ��ȫ������е�Ч��
    /// </summary>
    public void DisableAll();
}
