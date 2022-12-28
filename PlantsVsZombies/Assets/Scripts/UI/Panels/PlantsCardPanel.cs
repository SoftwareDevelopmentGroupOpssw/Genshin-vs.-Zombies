using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 植物卡槽界面
/// </summary>
public class PlantsCardPanel : BasePanel
{

    private int selected = -1;// 等于-1说明没有选中植物，为其他值则说明选中档位的植物

    private GameController controller;
    /// <summary>
    /// 生成的卡槽
    /// </summary>
    private List<Plot> plotList = new List<Plot>();

    [Header("使用的卡槽")]
    public GameObject plot;
    [Header("使用的卡槽遮罩")]
    public GameObject mask;
    public GridLayoutGroup area;
    public Transform energyLocation;
    private void OnEnergyChanged(int value) => GetControl<Text>("Energy").text = value.ToString();
    protected override void BeforeShow()
    {
        controller = GameController.Instance;
        if (controller.gameObject.activeSelf)
        {
            controller.EnergyMonitor.OnValueChanged += OnEnergyChanged;

            GetControl<Button>("ShovelBtn").onClick.AddListener(OnShovelClicked);
        }
        else
            Debug.LogError("The game is not started.");
    }
    protected override void BeforeHide()
    {
        controller.EnergyMonitor.OnValueChanged -= OnEnergyChanged;
        GetControl<Button>("ShovelBtn").onClick.RemoveAllListeners();

        SetPlotCount(0);
    }
    public void SetPlotCount(int count)
    {
        if (count == plotList.Count)
            return;

        foreach(Plot plot in plotList)
        {
            Destroy(plot.PlotObj.gameObject);
            Destroy(plot.Mask.gameObject);
        }
        plotList.Clear();

        RectTransform rect = transform as RectTransform;
        int basicWidth = 160;
        rect.sizeDelta = new Vector2(basicWidth + area.cellSize.x * count + area.padding.left + area.padding.right + area.spacing.x * (count - 1),rect.sizeDelta.y);
        for(int i = 0; i < count; i++)
        {
            //生成卡槽
            GameObject obj = Instantiate(plot, area.transform);
            Image objImage = obj.GetComponent<Image>();
            objImage.sprite = GameController.Instance.GetSelectPlant(i).Data.CardSprite;//设置卡槽图片
            obj.name = i.ToString();
            obj.GetComponent<Button>().onClick.AddListener(OnPlotClicked);
            //生成卡槽遮罩
            GameObject maskObj = Instantiate(mask, obj.transform);
            Image maskImage = maskObj.GetComponent<Image>();
            (maskImage.transform as RectTransform).sizeDelta = area.cellSize;//将遮罩的大小设置成与卡牌大小一致
            //添加储存
            plotList.Add(new Plot() { PlotObj = objImage,Mask = maskImage});
        }
    }

    private void OnPlotClicked()//点击时调用
    {
        int num = System.Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name);
        if (controller.CanPlacePlant(num) == null)//没有错误发生
            selected = num;
    }

    private void OnShovelClicked()
    {
        selected = plotList.Count;//如果有8个plot，则选中plot时范围为0-7，此时设置为8来表示为铲子
    }


    private GameObject real;//选择时，出现的实体植物
    private GameObject unreal;//选择时，出现的虚体植物

    /// <summary>
    /// 在选择植物时，产生实像虚像效果
    /// </summary>
    private void DisplayImageOnSelecting()
    {
        PlantsSelected selectPlant = GameController.Instance.GetSelectPlant(selected);
        //生成实像和虚像
        if (real == null)//不存在实像则创建
        {
            real = Instantiate(selectPlant.Data.OriginalReference);
            real.GetComponent<Plant>().enabled = false;//只是一个像，不需要启动逻辑功能
        }
        if(unreal == null)//不存在虚像则创建
        {
            unreal = Instantiate(selectPlant.Data.OriginalReference);
            unreal.GetComponent<Plant>().enabled = false;//只是一个像，不需要启动逻辑功能
        }
        //调整虚像透明度
        SpriteRenderer sprite = unreal.GetComponent<SpriteRenderer>();
        Color c = sprite.color; c.a = 0.5f; sprite.color = c;
        //获取游戏内现在加载的关卡对象位置和关卡数据
        ILevelData levelData = GameController.Instance.LevelData;
        Vector3 levelPos = GameController.Instance.Level.transform.position;
        //开始计算实像、虚像显示坐标
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        real.transform.position = new Vector3(worldPos.x, worldPos.y, 0);//实像坐标

        Vector2Int gridPos = levelData.WorldToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition), levelPos);
        if (gridPos != new Vector2Int(-1, -1))//在格子里才能显示虚像
        {
            unreal.SetActive(true);
            unreal.transform.position = levelData.GridToWorld(gridPos, GridPosition.Middle, levelPos);
        }
        else
            unreal.SetActive(false);
    }
    /// <summary>
    /// 刷新植物槽是否可用
    /// </summary>
    private void RefreshAvailableState()
    {
        for(int i = 0; i < plotList.Count; i++)
        {
            PlantAddException exception = controller.CanPlacePlant(i);
            if(exception == null)
            {
                plotList[i].PlotObj.color = Color.white;
                plotList[i].Mask.fillAmount = 0;
            }
            else
            {
                plotList[i].PlotObj.color = Color.grey;
                if (exception is PlantAddException.NotCooledDownYet)
                {
                    PlantsSelected selectedPlant = controller.GetSelectPlant(i);
                    plotList[i].Mask.fillAmount = selectedPlant.CooltimePercent;
                }
                else if (exception is PlantAddException.NotEnoughEnergy)
                {
                    plotList[i].Mask.fillAmount = 0;
                }
            }
        }
    }
    private void Update()
    {
        RefreshAvailableState();
        if(selected != -1)
            DisplayImageOnSelecting();
        else//摧毁实像和虚像
        {
            Destroy(real); real = null;
            Destroy(unreal); unreal = null;
        }

        //有植物被选中，卡牌显示为白色（没有变灰），按下鼠标左键时，进行操作
        if (selected != -1 && plotList[selected].PlotObj.color == Color.white && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2Int pixelPos = new Vector2Int((int)mousePos.x, (int)mousePos.y);
            if (selected != plotList.Count)//不是铲子
            {
                controller.PlacePlant(selected, pixelPos);
                selected = -1;
                Debug.Log("放置植物在" + pixelPos.ToString());
            }
            else
            {
                controller.RemovePlant(pixelPos);
                selected = -1;
                Debug.Log("移除植物" + pixelPos.ToString());
            }
        }
    }
    
    /// <summary>
    /// 卡槽数据信息
    /// </summary>
    struct Plot
    {
        /// <summary>
        /// 生成的卡槽对象
        /// </summary>
        public Image PlotObj;
        /// <summary>
        /// 卡槽中对冷却时间的遮罩
        /// </summary>
        public Image Mask;
    }
}
