using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��ѡ���ֲ�￨��
/// </summary>
public class PlantsSelected
{
 
    private IPlantData data;
    private float timeLeft;//ʣ��ʱ�䣨���룩

    /// <summary>
    /// ����һ����ѡ�񿨲۶���
    /// </summary>
    /// <param name="data">�����е�ֲ����Ϣ</param>
    public PlantsSelected(IPlantData data)
    {
        this.data = data;
        timeLeft = 0;
    }

    /// <summary>
    /// ֲ������
    /// </summary>
    public IPlantData Data => data;

    /// <summary>
    /// ���ֲ����ȴʱ����ȵİٷֱ�
    /// Ϊ1ʱ�տ�ʼ��ȴ��Ϊ0ʱ��ȴ���
    /// </summary>
    public float CooltimePercent => timeLeft / data.CoolTime;

    /// <summary>
    /// ��ʼ����ʱ��������ȴʱ��
    /// </summary>
    public void StartCoolTime()
    {
        void Update()//����monoģ��֡���º���
        {
            timeLeft -= Time.unscaledDeltaTime * 1000;
            if(timeLeft <= 0)
            {
                timeLeft = 0;
                MonoManager.Instance.RemoveUpdateListener(Update);
            }
        }
        timeLeft = data.CoolTime;//�趨�ÿ�ʼʱ��
        MonoManager.Instance.AddUpdateListener(Update);//��Update���빫��monoģ��
    }
}
