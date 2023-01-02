using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Ī�Ƚ�ɫ����
/// </summary>
public class MonaData : PlantData
{
    /// <summary>
    /// ��PlantSerializer���ã���ʼ������
    /// ��Ҫʹ�������������ʵ��������Ӧʹ��PlantSerializer�ķ���ȥ���һ���µ�����
    /// </summary>
    /// <param name="original">ԭʼ����</param>
    /// <param name="cardSprite">����ͼƬ</param>
    public MonaData(GameObject original,Sprite cardSprite):base(original,cardSprite)
    {
        
    }

    public override int EnergyCost => 100;

    private int health = 300;
    public override int Health { get => health; set => health = value; }

    private int atkPower = 20;
    public override int AtkPower { get => atkPower; set => atkPower = value; }

    public override int CoolTime => 7500;

    public override string PlantName => "Mona";

}
