using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ����ű�����
/// </summary>
public abstract class Monster : BaseGameobject
{
    public abstract IEffectHandler Handler { get; }
    public IMonsterData Data { get; set; }
    // Start is called before the first frame update
    /// <summary>
    /// ����֪ͨ�㲥
    /// </summary>
    public event UnityAction<Monster> OnDie;
    protected virtual void OnDestroy()
    {
        OnDie?.Invoke(this);
    }
}
