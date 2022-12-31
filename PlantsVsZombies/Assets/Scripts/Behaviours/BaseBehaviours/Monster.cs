using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ����ű�����
/// </summary>
public abstract class Monster : BaseGameobject
{
    /// <summary>
    /// Ч��������
    /// </summary>
    public abstract IEffectHandler Handler { get; }
    /// <summary>
    /// ħ��������Ϣ
    /// </summary>
    public IMonsterData Data { get; set; }

}
