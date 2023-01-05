using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidenShogun : Plant
{
    private Animator animator;
    private CountDown countDown;
    private DefaultHandler handler;
    private RaidenShogunData EnergyFlowerData => Data as RaidenShogunData;
    public override IEffectHandler Handler
    {
        get
        {
            if (handler == null)
                handler = new DefaultHandler(Data);
            return handler;
        }
    }
    AudioSource lastSource;
    private void PlayDamageingEffect()
    {
        float replayPercent = 0.5f;//�����Ž��ȴﵽ��ʱ����һ���ٷֱȾͿ�ʼ���²���
        if (lastSource == null || !lastSource.gameObject.activeSelf || lastSource.time > lastSource.clip.length * replayPercent)//������������ȥ�ˣ�����ֹͣ��
            lastSource = AudioManager.Instance.PlayRandomEffectAudio("chomp1", "chomp2");
    }
    private void Start()
    {
        Data.AddOnReceiveAllDamageListener((damage) => PlayDamageingEffect());

        animator = GetComponent<Animator>();
        countDown = new CountDown(EnergyFlowerData.ProduceDistance);
        countDown.OnComplete += PlayAnimate;
        countDown.StartCountDown();

        Invoke("PlayAnimate", 5f);//������ȥ��ʱ��ܿ���ܳ�һ������
    }
    void PlayAnimate()
    {
        animator.SetTrigger("Produce");
    }
    private void ProduceEnergy()
    {
        //�������ĳ���
        IEnumerator EnergyFlyingCoroutine(GameObject energy , Vector2 startPos)
        {
            void RealSetPosition(GameObject energyObj, Vector2 screenPoint)
            {
                RectTransform rect = energyObj.transform as RectTransform;
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

            //�����ĳ����ľ���
            float offset = 0.5f;
            float farestLocation = 50f;
            float nearestLocation = 10f;
            int sign = Random.value - offset > 0 ? 1 : -1;
            float now = Random.Range(sign * nearestLocation, sign * farestLocation);

            float flySpeed = 80;
            float polonomialArg = 0.05f;

            for (float x = 0; Mathf.Abs(x) < Mathf.Abs(now); x += sign * flySpeed * Time.deltaTime)
            {
                float y = -polonomialArg * x * (x - now);
                RealSetPosition(energy,startPos + new Vector2(x, y));
                yield return 1;
            }
        }

        Vector2 pixelPos = Camera.main.WorldToScreenPoint(transform.position);
        GameObject obj = EnergyMonitor.CreateEnergy(new Vector2Int((int)pixelPos.x, (int)pixelPos.y), EnergyFlowerData.ProduceType);

        StartCoroutine(EnergyFlyingCoroutine(obj,pixelPos));
        AudioManager.Instance.PlayRandomEffectAudio("throw1", "throw2");
        countDown.StartCountDown();
    }
    private void OnDestroy()
    {
        countDown.OnComplete -= PlayAnimate;
    }
}
