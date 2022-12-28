using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对于普通魔物的效果发生器
/// 当添加效果时，会自定义触发效果
/// </summary>
public class CommonMonsterHandler : IEffectHandler
{
    private IMonsterData data;
    public CommonMonsterHandler(IMonsterData data)
    {
        this.data = data;
    }

    public void EnableEffect(IEffect effect)
    {
        if (effect is Frozen)//当特殊效果为冻结效果时
        {

            effect.EnableEffect(data);//默认启用：启用减速和眩晕效果、碎冰解冻效果

        }
        else if(effect is ElectroCharged)
        {
            effect.EnableEffect(data);//默认启用：感电持续伤害、韧性降低、触发反应解除效果
        }
        else
        {
            effect.EnableEffect(data);
        }
    }

    public void UpdateEffect(IEffect effect)
    {
        effect.UpdateEffect(data);
    }
    public void DisableEffect(IEffect effect)
    {
        effect.DisableEffect(data);
    }

    public void CheckEffect(List<IEffect> effects)
    {
        List<IEffect> deletes = new List<IEffect>();
        foreach(IEffect effect in effects)
        {
            if(effect.State == EffectState.End)
            {
                deletes.Add(effect);
                effect.DisableEffect(data);
            }
            else if(effect.State == EffectState.Error)
            {
                deletes.Add(effect);
            }
        }
        foreach(IEffect delete in deletes)
        {
            effects.Remove(delete);  //移除
        }
        foreach(IEffect effect in effects)
        {
            switch (effect.State)
            {
                case EffectState.Initialized:
                    try
                    {
                        EnableEffect(effect);//尝试启动效果
                    }
                    catch (System.Exception e)//出错
                    {
                        Debug.LogException(e);//打印出错信息
                    }
                    break;
                case EffectState.Processing:
                    UpdateEffect(effect);
                    break;
                //出错和结束时藉由下次调用更新前剔除
            }
        }
    }
}
