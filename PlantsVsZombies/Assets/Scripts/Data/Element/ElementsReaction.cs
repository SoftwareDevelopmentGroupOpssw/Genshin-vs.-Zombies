using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementsReaction : MonoBehaviour
{
    private ElementsReaction()
    {

    }
    /// <summary>
    /// ��ȡһ��Ԫ�ط�Ӧ
    /// </summary>
    /// <param name="before">�Ѿ����ŵ�Ԫ��</param>
    /// <param name="after">�������ŵ�Ԫ��</param>
    /// <returns>Ԫ�ط�Ӧ�������Ԫ�ط�Ӧ��������Ϊ��</returns>
    public static ElementsReaction GetReaction(Elements before, Elements after)
    {
        //TODO:����ÿ�ֿ��е�Ԫ�ط�Ӧ������һ����Ӧ��Ԫ�ط�Ӧ��ȥ
        throw new System.NotImplementedException();
    }
    /// <summary>
    /// Ԫ�ط�Ӧ�ͷ�
    /// </summary>
    /// <param name="damage">Ԫ���˺���Դ</param>
    /// <param name="target">Ԫ�ط�ӦĿ��</param>
    public void Action(IElementalDamage damage, IMonsterData target)
    {

    }
}
