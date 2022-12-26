using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ч������ӿ�
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
}
