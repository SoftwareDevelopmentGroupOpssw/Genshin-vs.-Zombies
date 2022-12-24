using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����ű�����
/// </summary>
public abstract class Monster : MonoBehaviour
{
    /// <summary>
    /// ��������
    /// </summary>
    public IMonsterData Data { get; set; }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Data.OnAwake();
    }
    
    protected virtual void OnDestroy()
    {
        Data.OnDestroy();
    }
}
