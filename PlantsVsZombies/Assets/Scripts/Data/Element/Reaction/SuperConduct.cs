using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 超导
/// </summary>
public class SuperConduct : ElementsReaction, IEffect
{
    private static float radius = 0.8f;//超导效果触发的半径
    private static int damage = 20;//超导效果的伤害
    private static int duration = 6000;//超导效果持续时间
    private static float electricalResChange = -0.35f; //雷元素抗性降低
    private static float iceResChange = -0.35f; // 冰元素抗性降低

    private ResistanceEffect electricEffect = new ResistanceEffect(Elements.Electric, electricalResChange, duration, system); //雷元素抗性降低效果
    private ResistanceEffect iceEffect = new ResistanceEffect(Elements.Ice, iceResChange, duration, system);//冰元素抗性降低效果

    public override string ReactionName => "SuperConduct";

    public string EffectName => ReactionName;

    public EffectState State
    {
        get
        {
            return electricEffect.State; //使用雷元素抗性效果的状态作为超导效果的总状态
        }
    }

    public IGameobjectData Caster => system;




    protected override void RealAction(IElementalDamage damage, IDamageReceiver target)
    {
        //范围检测
        Collider2D[] colliders = Physics2D.OverlapCircleAll(target.GameObject.transform.position, radius);
        foreach (var collider in colliders)
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();
            if (damageable != null && damageable is Monster)
            {
                IDamageReceiver receiver = damageable.GetReceiver();
                if(receiver != null)
                {
                    //受到伤害
                    receiver.ReceiveDamage(new SystemDamage(SuperConduct.damage, Elements.Ice));
                    IEffect effect = receiver.GetEffects().Find((effect) => effect is SuperConduct);
                    if(effect != null) //身上没有超导效果，则添加
                    {

                        receiver.AddEffect(new SuperConduct());
                    }
                }
            }
        }
    }

    public void EnableEffect(IGameobjectData target)
    {
        electricEffect.EnableEffect(target);
        iceEffect.EnableEffect(target);
    }

    public void DisableEffect(IGameobjectData target)
    {
        electricEffect.DisableEffect(target);
        iceEffect.DisableEffect(target);
    }

    public void UpdateEffect(IGameobjectData target)
    {

    }
}
