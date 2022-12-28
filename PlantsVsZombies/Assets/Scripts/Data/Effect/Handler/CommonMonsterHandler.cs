using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������ͨħ���Ч��������
/// �����Ч��ʱ�����Զ��崥��Ч��
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
        if (effect is Frozen)//������Ч��Ϊ����Ч��ʱ
        {

            effect.EnableEffect(data);//Ĭ�����ã����ü��ٺ�ѣ��Ч��������ⶳЧ��

        }
        else if(effect is ElectroCharged)
        {
            effect.EnableEffect(data);//Ĭ�����ã��е�����˺������Խ��͡�������Ӧ���Ч��
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
}
