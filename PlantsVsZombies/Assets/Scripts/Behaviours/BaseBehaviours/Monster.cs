using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 怪物脚本基类
/// </summary>
public abstract class Monster : BaseGameobject
{
    public abstract IEffectHandler Handler { get; }
    public IMonsterData Data { get; set; }
    // Start is called before the first frame update
    /// <summary>
    /// 死亡通知广播
    /// </summary>
    public event UnityAction<Monster> OnDie;
    protected virtual void OnDestroy()
    {
        OnDie?.Invoke(this);
    }
}
