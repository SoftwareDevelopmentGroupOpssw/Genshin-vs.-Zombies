using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ֲ�����ݻ���
/// </summary>
public abstract class PlantData : IPlantData
{
    private List<IEffect> effects = new List<IEffect>();

    public abstract int EnergyCost { get; }

    public abstract int Health { get; set; }
    public abstract int AtkPower { get; set; }
    public GameObject GameObject { get; set; }

    /// <summary>
    /// ���๹�캯��������ֲ��Ԥ����Ϳ���ͼƬ
    /// </summary>
    /// <param name="original">Ԥ����</param>
    /// <param name="cardSprite">����ͼƬ</param>
    protected PlantData(GameObject original, Sprite cardSprite)
    {
        this.original = original;
        this.cardSprite = cardSprite;
    }
    
    protected GameObject original;
    public GameObject OriginalReference => original;

    protected Sprite cardSprite;
    public Sprite CardSprite => cardSprite;

    public abstract int CoolTime { get; }
    public abstract string PlantName { get; }

    public void AddEffect(IEffect effect) => effects.Add(effect);
    public void RemoveEffect(IEffect effect) => effects.Remove(effect);
    public List<IEffect> GetEffects() => effects;

    public void AddElement(Elements element)
    {

    }

    public void RemoveElement(Elements element)
    {

    }

    public Elements[] GetAllElements()
    {
        return new Elements[0];
    }

    private System.Action<IElementalDamage> onReceiveDamage;

    public bool ReceiveDamage(IElementalDamage damage)
    {
        onReceiveDamage?.Invoke(damage);
        Health -= damage.AtkDmg;
        return true;
    }

    public void AddOnReceiveDamageListener(Elements element, Action<IElementalDamage> action)
    {
        
    }

    public void RemoveOnReceiveDamageListener(Elements element, Action<IElementalDamage> action)
    {

    }

    public void AddOnReceiveAllDamageListener(Action<IElementalDamage> action)
    {
        onReceiveDamage += action;
    }

    public void RemoveOnReceiveAllDamageListener(Action<IElementalDamage> action)
    {
        onReceiveDamage -= action;
    }

    public void AddOnElementReactedListener(Elements element, Action<ElementsReaction> action)
    {
        
    }

    public void RemoveOnElementReactedListener(Elements element, Action<ElementsReaction> action)
    {
        
    }
}
