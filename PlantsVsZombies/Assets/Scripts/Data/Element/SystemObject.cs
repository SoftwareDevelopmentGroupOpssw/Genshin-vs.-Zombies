using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 是一个系统物体
/// 系统造成的效果可以使用这个类的instance来作为效果的施加者
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
