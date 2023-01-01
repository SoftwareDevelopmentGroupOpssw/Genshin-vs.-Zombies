using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 默认的效果发生器
/// 会将所有的效果都按默认效果开启
/// </summary>
public class DefaultHandler : IEffectHandler
{
    private IGameobjectData data;
    public DefaultHandler(IGameobjectData data)
    {
        this.data = data;
    }

    public void EnableEffect(IEffect effect)
    {
        effect.EnableEffect(data);
    }

    public void UpdateEffect(IEffect effect)
    {
        effect.UpdateEffect(data);
    }
    public void DisableEffect(IEffect effect)
    {
        effect.DisableEffect(data);
    }

    public void CheckEffect()
    {
        List<IEffect> effects = data.GetEffects();
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
    public void DisableAll()
    {
        foreach(var effect in data.GetEffects())
        {
            DisableEffect(effect);
        }
    }
}
