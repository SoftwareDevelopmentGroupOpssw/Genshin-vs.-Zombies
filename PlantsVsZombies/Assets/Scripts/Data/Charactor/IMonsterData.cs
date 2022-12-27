using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ħ��ӿ�
/// </summary>
public interface IMonsterData:ICharactorData
{
    /// <summary>
    /// ����ֵ
    /// </summary>
    public int Strength { get; set; }

    /// <summary>
    /// �ƶ��ٶ�
    /// </summary>
    public int Speed { get; set; }
    
    /// <summary>
    /// ��ȡ��ĳ��Ԫ�صĿ���
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <returns>����ֵ��0~1��</returns>
    public float GetResistance(Elements element);
    /// <summary>
    /// ���ö�ĳ��Ԫ�صĿ���
    /// </summary>
    /// <param name="value">��ֵ</param>
    /// <param name="element">Ԫ������</param>
    public void SetResistance(float value, Elements element);

    /// <summary>
    /// ���Ԫ�ظ���
    /// �˷�����ӵ�Ԫ�ز��ᴥ��Ԫ�ط�Ӧ
    /// </summary>
    /// <param name="element"></param>
    public void AddElement(Elements element);
    /// <summary>
    /// �Ƴ�Ԫ�ظ���
    /// �˷����Ƴ���Ԫ�ز��ᴥ���¼�
    /// </summary>
    /// <param name="element"></param>
    public void RemoveElement(Elements element);
    
    
    
    
    /// <summary>
    /// ��Ŀ�괦�ܵ��˺�
    /// �˺�������㿹�ԡ��˺�����Ӧ��ͬʱ����һЩ�¼��Ĵ���
    /// </summary>
    /// <param name="damage">�ܵ��˺�����Դ</param>
    public void ReceiveDamage(IElementalDamage damage);

    
    
    /// <summary>
    /// ����ܵ��˺�����
    /// </summary>
    /// <param name="element">�ܵ��˺���Ԫ������</param>
    /// <param name="action">�ܵ��˺�ʱ���õĺ���</param>
    public void AddOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action);
    /// <summary>
    /// �Ƴ��ܵ��˺�����
    /// </summary>
    /// <param name="element">�ܵ��˺���Ԫ������</param>
    /// <param name="action">�ܵ��˺�ʱ���õĺ���</param>
    public void RemoveOnReceiveDamageListener(Elements element, System.Action<IElementalDamage> action);

    /// <summary>
    /// ���Ԫ�ظ��ż���
    /// </summary>
    /// <param name="element">Ԫ�ظ���</param>
    /// <param name="action">�ܵ�Ԫ�ظ���ʱ���õĺ���</param>
    public void AddOnAddElementListener(Elements element, System.Action action);
    /// <summary>
    /// �Ƴ�Ԫ�ظ��ż���
    /// </summary>
    /// <param name="element">Ԫ�ظ���</param>
    /// <param name="action">�ܵ�Ԫ�ظ���ʱ���õĺ���</param>
    public void RemoveOnAddElementListener(Elements element, System.Action action);

    /// <summary>
    /// ���Ԫ�ط�Ӧ����
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <param name="action">Ԫ�ط�Ӧ����</param>
    public void AddOnElementReactedListener(Elements element, System.Action<ElementsReaction> action);
    /// <summary>
    /// �Ƴ�Ԫ�ط�Ӧ����
    /// </summary>
    /// <param name="element">Ԫ������</param>
    /// <param name="action">Ԫ�ط�Ӧ����</param>
    public void RemoveOnElementReactedListener(Elements element, System.Action<ElementsReaction> action);

}
