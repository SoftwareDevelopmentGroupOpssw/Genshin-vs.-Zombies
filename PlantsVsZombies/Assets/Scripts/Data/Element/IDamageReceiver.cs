using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �˺����������Զ�����Ԫ�ط�Ӧ��Ԫ���˺�
/// </summary>
public interface IDamageReceiver : ICharactorData
{
    /// <summary>
    /// ���Ԫ��
    /// </summary>
    /// <param name="element"></param>
    public void AddElement(Elements element);
    /// <summary>
    /// �Ƴ�Ԫ��
    /// </summary>
    /// <param name="element"></param>
    public void RemoveElement(Elements element);

    /// <summary>
    /// �������Ԫ�ظ���
    /// </summary>
    /// <returns></returns>
    public Elements[] GetAllElements();

    /// <summary>
    /// �ܵ�Ԫ���˺�
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>����˺��Ľ��</returns>
    public bool ReceiveDamage(IElementalDamage damage);

    /// <summary>
    /// ����ܵ�Ԫ���˺�����
    /// </summary>
    /// <param name="element"></param>
    /// <param name="action"></param>
    public void AddOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action);
    /// <summary>
    /// �Ƴ��ܵ��˺�����
    /// </summary>
    /// <param name="element"></param>
    /// <param name="action"></param>
    public void RemoveOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action);

    /// <summary>
    /// Ϊ�������͵�Ԫ���˺���Ӽ���
    /// </summary>
    /// <param name="action">�ܵ��˺�ʱ���õĺ���</param>
    public void AddOnReceiveAllDamageListener(System.Action<IElementalDamage> action);
    /// <summary>
    /// Ϊ�������͵�Ԫ���˺��Ƴ�����
    /// </summary>
    /// <param name="action">�ܵ��˺�ʱ���õĺ���</param>
    public void RemoveOnReceiveAllDamageListener(System.Action<IElementalDamage> action);

    /// <summary>
    /// ���Ԫ�ط�Ӧ����
    /// </summary>
    /// <param name="element">������Ԫ��</param>
    /// <param name="action">Ԫ����ʧʱ�����ķ�Ӧ</param>
    public void AddOnElementReactedListener(Elements element, System.Action<ElementsReaction> action);
    /// <summary>
    /// �Ƴ�Ԫ�ط�Ӧ����
    /// </summary>
    /// <param name="element">������Ԫ��</param>
    /// <param name="action">Ԫ����ʧʱ�����ķ�Ӧ</param>
    public void RemoveOnElementReactedListener(Elements element, System.Action<ElementsReaction> action);
}
