using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹基类，可以造成伤害
/// </summary>
public abstract class Bullet : Flyer, IElementalDamage
{
    public abstract int AtkDmg { get; set; }
    public abstract Elements ElementType { get; set; }
    public abstract bool CanAddElement { get; set; }
}
