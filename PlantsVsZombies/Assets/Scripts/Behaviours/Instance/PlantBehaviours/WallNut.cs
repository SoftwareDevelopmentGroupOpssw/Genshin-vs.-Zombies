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
        float replayPercent = 0.5f;//�����Ž��ȴﵽ��ʱ����һ���ٷֱȾͿ�ʼ���²���
        if (lastSource == null || !lastSource.gameObject.activeSelf || lastSource.time > lastSource.clip.length * replayPercent)//������������ȥ�ˣ�����ֹͣ��
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
