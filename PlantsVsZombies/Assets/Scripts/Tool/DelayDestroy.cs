using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 延迟一段时间后摧毁
/// </summary>
public class DelayDestroy : MonoBehaviour
{
    /// <summary>
    /// 延迟时间（毫秒）
    /// </summary>
    public int DelayMiliseconds = 5000;
    public void Start()
    {
        Invoke("DestroyObj", (float)DelayMiliseconds / 1000);

    }
    void DestroyObj()
    {
        Destroy(gameObject);    
    }
}
