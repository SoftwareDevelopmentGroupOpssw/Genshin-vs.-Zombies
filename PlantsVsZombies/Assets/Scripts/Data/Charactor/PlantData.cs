using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlantData : IPlantData
{
    private List<IEffect> effects = new List<IEffect>();
    public abstract bool isReady { get; }

    public abstract int EnergyCost { get; }

    public abstract int Health { get; set; }
    public abstract int AtkPower { get; set; }
    public GameObject GameObject { get; set; }

    public abstract GameObject OriginalReference { get; }

    public bool CanAction { get; set; }
    public abstract Sprite CardSprite { get; }
    public abstract int CoolTime { get; }

    public void AddEffect(IEffect effect) => effects.Add(effect);
    public void RemoveEffect(IEffect effect) => effects.Remove(effect);

    public abstract void Action();

    public abstract void OnAwake();

    public abstract void OnDestroy();
}
