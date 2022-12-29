using System.Collections;
using System.Collections.Generic;
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
    public void Start()
    {
        Invoke("DestroyObj", (float)DelayMiliseconds / 1000);

    }
    void DestroyObj()
    {
        Destroy(gameObject);    
    }
}
