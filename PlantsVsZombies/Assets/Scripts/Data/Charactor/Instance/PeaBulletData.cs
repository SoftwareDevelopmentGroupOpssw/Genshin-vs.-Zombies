using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ͨ�÷��������ݣ��㶹
/// </summary>
public class PeaBulletData : IFlyerData
{
    public PeaBulletData(GameObject original)
    {
        OriginalReference = original;
    }

    public GameObject GameObject { get; set; }

    public GameObject OriginalReference { get; private set; }

    
    /// <summary>
    /// ��չ���㶹���Լ�Ч��
    /// </summary>
    /// <param name="effect"></param>
    public void AddEffect(IEffect effect)
    {
        
    }
    /// <summary>
    /// ��չ���㶹���Ի�����е�Ч���б�
    /// </summary>
    /// <returns></returns>
    public List<IEffect> GetEffects()
    {
        return null;
    }
    /// <summary>
    /// ��չ���㶹�����Ƴ�Ч��
    /// </summary>
    /// <param name="effect"></param>
    public void RemoveEffect(IEffect effect)
    {
        
    }
}
