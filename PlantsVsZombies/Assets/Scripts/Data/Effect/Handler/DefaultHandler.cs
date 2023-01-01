using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Ĭ�ϵ�Ч��������
/// �Ὣ���е�Ч������Ĭ��Ч������
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
            effects.Remove(delete);  //�Ƴ�
        }
        foreach(IEffect effect in effects)
        {
            switch (effect.State)
            {
                case EffectState.Initialized:
                    try
                    {
                        EnableEffect(effect);//��������Ч��
                    }
                    catch (System.Exception e)//����
                    {
                        Debug.LogException(e);//��ӡ������Ϣ
                    }
                    break;
                case EffectState.Processing:
                    UpdateEffect(effect);
                    break;
                //����ͽ���ʱ�����´ε��ø���ǰ�޳�
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
