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
    private void Start()
    {
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
