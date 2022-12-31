using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ֲ��ű�����
/// </summary>
public abstract class Plant : BaseGameobject,IMonsterAttackable
{
    /// <summary>
    /// ֲ��������Ϣ
    /// </summary>
    public IPlantData Data { get; set; }

    protected virtual void Update()
    {
        if (Data.Health <= 0)
        {
            GameController.Instance.PlantsController.RemovePlant(this);

            return;
        }
    }
 

    public ICharactorData GetData()
    {
        return Data;
    }
}
