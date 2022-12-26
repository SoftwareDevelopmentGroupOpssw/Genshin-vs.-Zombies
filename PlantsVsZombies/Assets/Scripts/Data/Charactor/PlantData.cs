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

    public abstract string ResourcePath { get; }

    public void AddEffect(IEffect effect) => effects.Add(effect);

    public abstract void Action();

    public abstract IGameobjectData Instantiate();

    public abstract void OnAwake();

    public abstract void OnDestroy();
}
