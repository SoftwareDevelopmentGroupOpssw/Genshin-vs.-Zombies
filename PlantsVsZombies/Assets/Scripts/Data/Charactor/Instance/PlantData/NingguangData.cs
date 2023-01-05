using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NingguangData : PlantData
{
    public NingguangData(GameObject original, Sprite cardSprite) : base(original, cardSprite)
    {
    }

    public override int EnergyCost => 300;

    public override int Health { get; set; } = 300;
    public override int AtkPower { get; set; } = 80;

    public override int CoolTime => 12000;

    public override string PlantName => "Ningguang";

    public override string Description => "凝光：能隔一段时间向前方扔一个岩石，岩石在接触第一个敌人时会碎裂，对第一个敌人造成岩属性伤害，并对周围的敌人造成溅射伤害。";
}
