using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 能量按钮的脚本逻辑
/// </summary>
public class Energy : MonoBehaviour
{
    private bool isClicked = false;//是否被点击
    [Header("能量值")]
    public int energyValue;
    [Header("消失时间(毫秒)")]
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
    private IEnumerator DestroyCoroutine()//缓慢摧毁的协程
    {
        //淡出效果逻辑
        int disappearSpeed = 2;
        Image image = GetComponent<Image>();
        for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime * disappearSpeed)
        {
            Color c = image.color;
            c.a = alpha;
            image.color = c;
            yield return 1;
        }
        //摧毁
        EnergyMonitor.DestroyEnergy(this);
    }
    private IEnumerator DisappearCoroutine()//缓慢消失的协程
    {
        //淡出效果逻辑
        int disappearSpeed = 2;
        Image image = GetComponent<Image>();
        for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime * disappearSpeed)
        {
            if (isClicked)//正在飞行，因此不透明度调到最大
            {
                yield break;//结束，等到飞行到了再调用destroy
            }
            else
            {
                Color c = image.color;
                c.a = alpha;
                image.color = c;
                yield return 1;
            }
        }
        //摧毁
        EnergyMonitor.DestroyEnergy(this);
    }
    /// <summary>
    /// 慢慢移动
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
            yield return 1;//下次帧更新
        }
        transform.position = pos;//直接传送到终点
        GameController.Instance.EnergyMonitor.AddEnergy(energyValue);

        StartCoroutine(DestroyCoroutine());
    }
    /// <summary>
    /// 给外部使用 让生成出来的能量往下掉落一段距离
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
        //没有被点击过
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
        if (countDown.Available && !isClicked)//时间到了，也没有被点击飞行
            StartCoroutine(DisappearCoroutine());
    }
}
