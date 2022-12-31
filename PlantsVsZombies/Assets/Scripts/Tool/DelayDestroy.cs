using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// �ӳ�һ��ʱ���ݻ�
/// </summary>
public class DelayDestroy : MonoBehaviour
{
    /// <summary>
    /// �ӳ�ʱ�䣨���룩
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
