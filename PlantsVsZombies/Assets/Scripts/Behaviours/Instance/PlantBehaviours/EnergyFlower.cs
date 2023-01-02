using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyFlower : Plant
{
    private CountDown countDown;
    private DefaultHandler handler;
    private EnergyFlowerData EnergyFlowerData => Data as EnergyFlowerData;
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
        countDown = new CountDown(EnergyFlowerData.ProduceDistance);
        countDown.OnComplete += ProduceEnergy;

        Invoke("ProduceEnergy", 5f);
    }
    private void ProduceEnergy()
    {
        //�������ĳ���
        IEnumerator EnergyFlyingCoroutine(GameObject energy)
        {
            //�����ĳ����ľ���
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

        countDown.StartCountDown();
    }
    private void OnDestroy()
    {
        countDown.OnComplete -= ProduceEnergy;
    }
}
