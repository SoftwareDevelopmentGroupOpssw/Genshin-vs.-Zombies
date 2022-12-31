using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 怪物脚本基类
/// </summary>
public abstract class Monster : BaseGameobject
{
    public abstract IEffectHandler Handler { get; }
    public override IGameobjectData Data { get; set; }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }
    
    protected virtual void OnDestroy()
    {
        
    }
}
