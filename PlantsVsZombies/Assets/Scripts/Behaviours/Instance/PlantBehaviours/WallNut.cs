using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallNut : Plant
{
    private int maxHealth;
    private int lastStateHealth;
    private DefaultHandler handler;
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
            lastSource = AudioManager.Instance.PlayEffectAudio("chompsoft");
    }
    private void Start()
    {
        Data.AddOnReceiveAllDamageListener((damage) => PlayDamageingEffect());

        maxHealth = Data.Health;
        lastStateHealth = Data.Health;
    }
    protected override void Update()
    {
        if((lastStateHealth - Data.Health) > maxHealth * 0.33f)
        {
            GetComponent<Animator>().SetTrigger("Break");
            lastStateHealth = (int)(lastStateHealth - maxHealth * 0.33f);
        }
        base.Update();
    }
}
