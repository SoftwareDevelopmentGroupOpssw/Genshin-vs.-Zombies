using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 怪物脚本基类
/// </summary>
public abstract class Monster : MonoBehaviour
{
    /// <summary>
    /// 怪物数据
    /// </summary>
    public IMonsterData Data { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
