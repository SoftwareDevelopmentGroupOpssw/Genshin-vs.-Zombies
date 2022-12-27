using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 是一个系统物体
/// 系统造成的效果可以使用这个类的instance来表明是系统造成的效果
/// </summary>
public class SystemObject : Singleton<SystemObject>, IGameobjectData
{
    private static GameObject system = new GameObject("System");

    public GameObject GameObject { get => system; set { } }

    public GameObject OriginalReference => system;

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
