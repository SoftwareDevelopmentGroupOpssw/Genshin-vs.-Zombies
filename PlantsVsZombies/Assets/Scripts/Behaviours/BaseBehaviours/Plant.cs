using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ֲ��ű�����
/// </summary>
public abstract class Plant : MonoBehaviour
{
    /// <summary>
    /// ֲ�������
    /// </summary>
    public IPlantData Data { get; set; }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Data.OnAwake();
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if (Data.isReady)
        {
            Data.Action();
        }
    }
    protected virtual void OnDestroy()
    {
        Data.OnDestroy();
    }
}
