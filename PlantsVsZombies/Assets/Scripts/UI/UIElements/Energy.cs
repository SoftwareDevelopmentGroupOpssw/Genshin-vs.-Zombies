using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Energy : MonoBehaviour
{
    public int energyValue;
    // Start is called before the first frame update
    void Start()
    {
        if (GameController.Instance.IsGameStarted)
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
        }
        else
            Debug.LogError("The game is not started.");
    }
    private IEnumerator Coroutine()
    {
        Transform rectEnd = UIManager.Instance
            .GetPanel<SelectPanel>("SelectPanel")
            .energyLocation
            .transform;
        Vector3 pos = rectEnd.position;
        Vector3 startPos = transform.position;
        int speed = 5;
        float totalTime = 1;
        for(float i = 0; i < totalTime;i += Time.deltaTime * speed)
        {
            transform.position = Vector3.Lerp(startPos, pos, i);
            yield return 1;//下次帧更新
        }
        GameController.Instance.Energy.AddEnergy(energyValue);

        //淡出效果逻辑
        int disappearSpeed = 3;
        Image image = GetComponent<Image>();
        for(float alpha = 1;alpha > 0;alpha -= Time.deltaTime * disappearSpeed)
        {
            Color c = image.color;
            c.a = alpha;
            image.color = c;
            yield return 1; 
        }
        Destroy(gameObject);
    }
    private bool isFlying = false;//正在移动中
    private void OnClicked()
    {
        if (!isFlying)
        {
            StartCoroutine(Coroutine());
            isFlying = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
