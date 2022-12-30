using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
    public virtual void Start()
    {
        Task.Run(() =>
        {
            Thread.Sleep(DelayMiliseconds);
            DestroyThis();
        });

    }
    protected void DestroyThis()
    {
        Destroy(gameObject);    
    }
}
