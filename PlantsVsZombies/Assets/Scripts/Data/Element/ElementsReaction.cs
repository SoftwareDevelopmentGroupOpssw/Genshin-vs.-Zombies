using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ԫ�ط�Ӧ����
/// </summary>
public abstract class ElementsReaction : MonoBehaviour
{
    /// <summary>
    /// ��ȡһ��Ԫ�ط�Ӧ
    /// </summary>
    /// <param name="before">�Ѿ����ŵ�Ԫ��</param>
    /// <param name="after">�������ŵ�Ԫ��</param>
    /// <returns>Ԫ�ط�Ӧ�������Ԫ�ط�Ӧ��������Ϊ��</returns>
    public static ElementsReaction GetReaction(Elements before, Elements after)
    {
        //TODO:����ÿ�ֿ��е�Ԫ�ط�Ӧ������һ����Ӧ��Ԫ�ط�Ӧ��ȥ
        //���Ԫ�ط�Ӧ��Ȼ����Ҫ�̳�ElementsReaction�ಢ��дAction����
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Ԫ�ط�Ӧ�ͷ�
    /// </summary>
    /// <param name="damage">Ԫ���˺���Դ</param>
    /// <param name="target">Ԫ�ط�ӦĿ��</param>
    public abstract void Action(IElementalDamage damage, IMonsterData target);
}
