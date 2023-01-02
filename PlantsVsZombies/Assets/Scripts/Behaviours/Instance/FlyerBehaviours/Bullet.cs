using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹基类，可以造成伤害
/// </summary>
public abstract class Bullet : Flyer, IElementalDamage
{
    /// <summary>
    /// 每次获取子弹伤害时都会创建一个新实例防止修改
    /// </summary>
    protected BulletDamage bulletDamage => new BulletDamage() { Damage = damage, ElementType = element, CanAddElement = canAddElement };


    [Header("子弹对接触到的目标的伤害")]
    [SerializeField]
    protected int damage;
    public int Damage { get => damage; set => damage = value; }
    [Header("子弹的元素类型")]
    [SerializeField]
    protected Elements element;
    public Elements ElementType { get => element; set => element = value; }

    [Header("子弹能否添加元素")]
    [SerializeField]
    protected bool canAddElement;
    public bool CanAddElement { get => canAddElement; set => canAddElement = value; }


    //从IElementalDamage的接口
    int IElementalDamage.Damage { get => bulletDamage.Damage; set => bulletDamage.Damage = value; }
    Elements IElementalDamage.ElementType { get => bulletDamage.ElementType; set => bulletDamage.ElementType = value; }
    bool IElementalDamage.CanAddElement { get => bulletDamage.CanAddElement; set => bulletDamage.CanAddElement = value; }

    protected class BulletDamage : IElementalDamage
    {
        public int Damage { get; set; }
        public Elements ElementType { get; set; }
        public bool CanAddElement { get; set; }
    }
}
