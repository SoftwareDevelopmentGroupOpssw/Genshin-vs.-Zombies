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
}
