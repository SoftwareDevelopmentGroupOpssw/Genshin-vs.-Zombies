using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ������ť�Ľű��߼�
/// </summary>
public class Energy : MonoBehaviour
{
    [Header("����ֵ")]
    public int energyValue;
    [Header("��ʧʱ��(����)")]
    public int disappearTime;
    private CountDown countDown;
    public EnergyType EnerygyType { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        if (GameController.Instance.IsGameStarted)
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
            countDown = new CountDown(disappearTime);
            countDown.StartCountDown();
        }
        else
            Debug.LogError("The game is not started.");
    }
    private IEnumerator DestroyCoroutine()
    {
        //����Ч���߼�
        int disappearSpeed = 2;
        Image image = GetComponent<Image>();
        for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime * disappearSpeed)
        {
            Color c = image.color;
            if (isFlying)//���ڷ��У���˲�͸���ȵ������
            {
                c.a = 1;
                image.color = c;
                yield break;//�������ȵ����е����ٵ���destroy
            }
            else
            {
                c.a = alpha;
                image.color = c;
                yield return 1;
            }
        }
        //�ݻ�
        EnergyMonitor.DestroyEnergy(this);
    }
    /// <summary>
    /// �����ƶ�
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlyingCoroutine()
    {
        isFlying = true;
        Transform rectEnd = UIManager.Instance
            .GetPanel<PlantsCardPanel>("PlantsCardPanel")
            .energyLocation
            .transform;
        Vector3 pos = rectEnd.position;
        Vector3 startPos = transform.position;
        int speed = 5;
        float totalTime = 1;
        for(float i = 0; i < totalTime;i += Time.deltaTime * speed)
        {
            transform.position = Vector3.Lerp(startPos, pos, i);
            yield return 1;//�´�֡����
        }
        transform.position = pos;//ֱ�Ӵ��͵��յ�
        GameController.Instance.EnergyMonitor.AddEnergy(energyValue);

        isFlying = false;
        StartCoroutine(DestroyCoroutine());
    }
    private bool isFlying = false;//�����ƶ���
    private void OnClicked()
    {
        //�����е���������󲻴����߼�
        if (!isFlying)
        {
            Image image = GetComponent<Image>();
            Color c = image.color;
            c.a = 1;
            image.color = c;

            StartCoroutine(FlyingCoroutine());
        }
    }

    // Update is called once per frame
    void Update()
    { 
        if (countDown.Available && !isFlying)//ʱ�䵽�ˣ�Ҳû�б��������
            StartCoroutine(DestroyCoroutine());
    }
}
