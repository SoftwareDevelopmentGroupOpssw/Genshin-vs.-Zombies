using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ӵ����࣬��������˺�
/// </summary>
public abstract class Bullet : Flyer, IElementalDamage
{
    /// <summary>
    /// ÿ�λ�ȡ�ӵ��˺�ʱ���ᴴ��һ����ʵ����ֹ�޸�
    /// </summary>
    protected BulletDamage bulletDamage => new BulletDamage() { Damage = damage, ElementType = element, CanAddElement = canAddElement };


    [Header("�ӵ��ԽӴ�����Ŀ����˺�")]
    [SerializeField]
    protected int damage;
    public int Damage { get => damage; set => damage = value; }
    [Header("�ӵ���Ԫ������")]
    [SerializeField]
    protected Elements element;
    public Elements ElementType { get => element; set => element = value; }

    [Header("�ӵ��ܷ����Ԫ��")]
    [SerializeField]
    protected bool canAddElement;
    public bool CanAddElement { get => canAddElement; set => canAddElement = value; }


    //��IElementalDamage�Ľӿ�
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
