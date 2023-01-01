using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹基类，可以造成伤害
/// </summary>
public abstract class Bullet : Flyer, IElementalDamage
{
    protected abstract BulletDamage bulletDamage { get; }

    public int AtkDmg { get => bulletDamage.AtkDmg; set => bulletDamage.AtkDmg = value; }
    public Elements ElementType { get => bulletDamage.ElementType; set => bulletDamage.ElementType = value; }
    public bool CanAddElement { get => bulletDamage.CanAddElement; set => bulletDamage.CanAddElement = value; }

    protected class BulletDamage : IElementalDamage
    {
        public int AtkDmg { get; set; }
        public Elements ElementType { get; set; }
        public bool CanAddElement { get; set; }
    }
}
