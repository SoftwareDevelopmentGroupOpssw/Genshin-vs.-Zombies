using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public MonaData(GameObject original,Sprite cardSprite)
    {
        this.original = original;
        this.cardSprite = cardSprite;
    }

    private CountDown countDown = new CountDown(2000);
    public override bool isReady => countDown.Available;

    public override int EnergyCost => 100;

    private int health = 100;
    public override int Health { get => health; set => health = value; }

    private int atkPower;
    public override int AtkPower { get => atkPower; set => atkPower = value; }

    private GameObject original;
    public override GameObject OriginalReference { get => original; }

    private Sprite cardSprite;
    public override Sprite CardSprite => cardSprite;

    public override int CoolTime => 5;

    public override void Action()
    {
        Debug.Log("Ī�ȿ�ʼ����");
    }

    public override void OnAwake()
    {
        Debug.Log("Ī�ȳ���");
    }

    public override void OnDestroy()
    {
        Debug.Log("Ī������");
    }
}
