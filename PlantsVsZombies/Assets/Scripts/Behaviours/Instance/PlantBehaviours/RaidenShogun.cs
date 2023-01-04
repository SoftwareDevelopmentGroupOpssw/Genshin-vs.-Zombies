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
        float replayPercent = 0.5f;//当播放进度达到总时长的一定百分比就开始重新播放
        if (lastSource == null || !lastSource.gameObject.activeSelf || lastSource.time > lastSource.clip.length * replayPercent)//被塞到池子里去了，播放停止了
            lastSource = AudioManager.Instance.PlayRandomEffectAudio("chomp1", "chomp2");
    }
    private void Start()
    {
        Data.AddOnReceiveAllDamageListener((damage) => PlayDamageingEffect());

        animator = GetComponent<Animator>();
        countDown = new CountDown(EnergyFlowerData.ProduceDistance);
        countDown.OnComplete += PlayAnimate;
        countDown.StartCountDown();

        Invoke("PlayAnimate", 5f);//刚种下去的时候很快就能出一个阳光
    }
    void PlayAnimate()
    {
        animator.SetTrigger("Produce");
    }
    private void ProduceEnergy()
    {
        //让能量蹦出来
        IEnumerator EnergyFlyingCoroutine(GameObject energy)
        {
            //能量蹦出来的距离
            float offset = 0.5f;
            float farestLocation = 50f;
            float nearestLocation = 10f;
            int sign = Random.value - offset > 0 ? 1 : -1;
            float now = Random.Range(sign * nearestLocation, sign * farestLocation);

            Vector3 startPos = energy.transform.position;
            float flySpeed = 80;
            float polonomialArg = 0.05f;

            for (float x = 0; Mathf.Abs(x) < Mathf.Abs(now); x += sign * flySpeed * Time.deltaTime)
            {
                float y = -polonomialArg * x * (x - now);
                energy.transform.position = startPos + new Vector3(x, y, 0);
                yield return 1;
            }
        }

        Vector2 pixelPos = Camera.main.WorldToScreenPoint(transform.position);
        GameObject obj = EnergyMonitor.CreateEnergy(new Vector2Int((int)pixelPos.x, (int)pixelPos.y), EnergyFlowerData.ProduceType);

        StartCoroutine(EnergyFlyingCoroutine(obj));
        AudioManager.Instance.PlayRandomEffectAudio("throw1", "throw2");
        countDown.StartCountDown();
    }
    private void OnDestroy()
    {
        countDown.OnComplete -= PlayAnimate;
    }
}
