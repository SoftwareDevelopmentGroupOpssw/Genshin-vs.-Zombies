using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 怪物脚本基类
/// </summary>
public abstract class Monster : BaseGameobject
{
    /// <summary>
    /// 效果处理器
    /// </summary>
    public abstract IEffectHandler Handler { get; }
    /// <summary>
    /// 魔物数据信息
    /// </summary>
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
