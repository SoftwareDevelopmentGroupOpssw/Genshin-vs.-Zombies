using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��һ��ϵͳ����
/// ϵͳ��ɵ�Ч������ʹ��������instance��������ϵͳ��ɵ�Ч��
/// </summary>
public class SystemObject : Singleton<SystemObject>, IGameobjectData
{
    private static GameObject system = new GameObject("System");

    public GameObject GameObject { get => system; set { } }

    public string ResourcePath => "";

    public void AddEffect(IEffect effect)
    {
        
    }

    public IGameobjectData Instantiate()
    {
        return this;
    }

    public void OnAwake()
    {
        
    }

    public void OnDestroy()
    {
        
    }

    public void RemoveEffect(IEffect effect)
    {
        
    }
}
