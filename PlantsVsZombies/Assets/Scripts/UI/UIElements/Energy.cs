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
    private bool isClicked = false;//�Ƿ񱻵��
    [Header("����ֵ")]
    public int energyValue;
    [Header("��ʧʱ��(����)")]
    public int disappearTime;
    private CountDown countDown;
    private CountDown CountDown
    {
        get
        {
            if (countDown == null)
                countDown = new CountDown(disappearTime);
            return countDown;
        }
    }
    public EnergyType EnerygyType { get; set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (GameController.Instance.IsGameStarted)
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
        }
        else
            Debug.LogError("The game is not started.");
    }
    private void OnEnable()
    {
        GetComponent<Image>().color = Color.white;
        isClicked = false;
        CountDown.StartCountDown();
    }
    private void OnDisable()
    {
        CountDown.Reset();
    }
    private IEnumerator DestroyCoroutine()//�����ݻٵ�Э��
    {
        //����Ч���߼�
        int disappearSpeed = 2;
        Image image = GetComponent<Image>();
        for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime * disappearSpeed)
        {
            Color c = image.color;
            c.a = alpha;
            image.color = c;
            yield return 1;
        }
        //�ݻ�
        EnergyMonitor.DestroyEnergy(this);
    }
    private IEnumerator DisappearCoroutine()//������ʧ��Э��
    {
        //����Ч���߼�
        int disappearSpeed = 2;
        Image image = GetComponent<Image>();
        for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime * disappearSpeed)
        {
            if (isClicked)//���ڷ��У���˲�͸���ȵ������
            {
                yield break;//�������ȵ����е����ٵ���destroy
            }
            else
            {
                Color c = image.color;
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

        StartCoroutine(DestroyCoroutine());
    }
    /// <summary>
    /// ���ⲿʹ�� �����ɳ������������µ���һ�ξ���
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="startScreenPos"></param>
    /// <param name="endScreenPos"></param>
    /// <returns></returns>
    public IEnumerator FallingCoroutine(GameObject obj, Vector2 startScreenPos,Vector2 endScreenPos)
    {
        void RealSetPosition(Vector2 screenPoint)
        {
            RectTransform rect = transform as RectTransform;
            Vector3 worldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle
                (
                    UIManager.Instance.Canvas.transform as RectTransform,
                    screenPoint,
                    null,
                    out worldPos
                );
            rect.position = worldPos;
        }
        
        float fallingSpeed = 0.5f;
        for (float i = 0; i < 1; i += Time.deltaTime * fallingSpeed)
        {
            RealSetPosition(Vector3.Lerp(startScreenPos, endScreenPos, i));
            if (isClicked)
                yield break;
            yield return 1;
        }

}
    private void OnClicked()
    {
        //û�б������
        if (!isClicked)
        {
            isClicked = true;
            Image image = GetComponent<Image>();
            Color c = image.color;
            c.a = 1;
            image.color = c;

            StartCoroutine(FlyingCoroutine());

            AudioManager.Instance.PlayEffectAudio("points");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown.Available && !isClicked)//ʱ�䵽�ˣ�Ҳû�б��������
            StartCoroutine(DisappearCoroutine());
    }
}
