using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��һ��ϵͳ����
/// ϵͳ��ɵ�Ч������ʹ��������instance����ΪЧ����ʩ����
/// </summary>
public class SystemObject : Singleton<SystemObject>, IGameobjectData
{
    private static GameObject system = new GameObject("System");

    public GameObject GameObject { get => system; set { } }

    public GameObject OriginalReference => system;

    public void AddEffect(IEffect effect)
    {
        
    }

    public List<IEffect> GetEffects()
    {
        return null;
    }

    public void RemoveEffect(IEffect effect)
    {
        
    }
}
