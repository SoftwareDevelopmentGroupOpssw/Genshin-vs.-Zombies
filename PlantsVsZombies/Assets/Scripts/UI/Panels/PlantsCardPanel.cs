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
    private List<GameObject> plotList = new List<GameObject>();

    public GameObject plot;
    public GridLayoutGroup area;
    public Transform energyLocation;
    private void OnEnergyChanged(int value) => GetControl<Text>("Energy").text = value.ToString();
    protected override void BeforeShow()
    {
        controller = GameController.Instance;
        if (controller.gameObject.activeSelf)
        {
            controller.Energy.OnValueChanged += OnEnergyChanged;

            GetControl<Button>("ShovelBtn").onClick.AddListener(OnShovelClicked);
        }
        else
            Debug.LogError("The game is not started.");
    }
    protected override void BeforeHide()
    {
        controller.Energy.OnValueChanged -= OnEnergyChanged;
        GetControl<Button>("ShovelBtn").onClick.RemoveAllListeners();

        SetPlotCount(0);
    }
    public void SetPlotCount(int count)
    {
        if (count == plotList.Count)
            return;

        foreach(GameObject plot in plotList)
        {
            Destroy(plot);
        }
        plotList.Clear();

        RectTransform rect = transform as RectTransform;
        int basicWidth = 160;
        rect.sizeDelta = new Vector2(basicWidth + area.cellSize.x * count + area.padding.left + area.padding.right + area.spacing.x * (count - 1),rect.sizeDelta.y);
        for(int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(plot, area.transform);
            obj.GetComponent<Image>().sprite = GameController.Instance.GetSelectPlant(i).Data.CardSprite;//设置卡槽图片
            obj.name = i.ToString();
            obj.GetComponent<Button>().onClick.AddListener(OnPlotClicked);
            plotList.Add(obj);
        }
    }
    private void OnPlotClicked()
    {
        selected = System.Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name);
    }
    private void OnShovelClicked()
    {
        selected = plotList.Count;//如果有8个plot，则选中plot时范围为0-7，此时设置为8来表示为铲子
    }


    private GameObject real;//选择时，出现的实体植物
    private GameObject unreal;//选择时，出现的虚体植物

    private void DisplayImageOnSelecting()//在选择植物时，产生实像虚像效果
    {
        PlantsSelected selectPlant = GameController.Instance.GetSelectPlant(selected);
        //生成实像和虚像
        if(real == null)//不存在实像则创建
            real = Instantiate(selectPlant.Data.OriginalReference);
        if(unreal == null)//不存在虚像则创建
            unreal = Instantiate(selectPlant.Data.OriginalReference);
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

    private void Update()
    {
        if(selected != -1)
        {
            DisplayImageOnSelecting();
        }
        else//摧毁实像和虚像
        {
            Destroy(real); real = null;
            Destroy(unreal); unreal = null;
        }

        if (selected != -1 && Input.GetMouseButtonDown(0))//当有植物被选中且再次按下左键时，判断逻辑
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2Int pixelPos = new Vector2Int((int)mousePos.x, (int)mousePos.y);
            if (selected != plotList.Count)//不是铲子
            {
                if (controller.TryPlacePlant(selected, pixelPos))
                {
                    selected = -1;
                    Debug.Log("放置植物在" +pixelPos.ToString());
                }
            }
            else
            {
                if (controller.TryRemovePlant(pixelPos))
                {
                    selected = -1;
                    Debug.Log("移除植物" + pixelPos.ToString());
                }
            }
        }
    }
}
