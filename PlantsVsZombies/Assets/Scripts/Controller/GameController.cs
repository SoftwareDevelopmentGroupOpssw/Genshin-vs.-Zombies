using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 游戏控制器
/// 当程序运行时作为物体的脚本自动被添加到场景中，只有一个实例
/// 当游戏进行中时为Active，游戏不在进行中时为Disactive
/// </summary>
public class GameController : MonoBehaviour
{
    private static GameController instance;
    /// <summary>
    /// 单例
    /// </summary>
    public static GameController Instance => instance;
    
    /// <summary>
    /// 更新模块，负责场景上的帧更新
    /// </summary>
    private Updater updater;
    
    /// <summary>
    /// 关卡信息
    /// </summary>
    public ILevelData LevelData { get; set; }//当选择关卡完成时，设置关卡

    [SerializeField]
    private GameObject level;
    /// <summary>
    /// 装载着地图Sprite的游戏对象
    /// </summary>
    public GameObject Level { get => level; }

    private PlantsController plantsController;
    /// <summary>
    /// 所有植物的控制器
    /// </summary>
    public PlantsController PlantsController => plantsController;

    private FlyersController flyerController;
    /// <summary>
    /// 所有飞行物的控制器
    /// </summary>
    public FlyersController FlyersController => flyerController;

    private MonstersController monsterController;
    /// <summary>
    /// 所有魔物的控制器
    /// </summary>
    public MonstersController MonstersController => monsterController;
    
    /// <summary>
    /// 已选择的植物
    /// </summary>
    private List<PlantsSelected> selected = new List<PlantsSelected>();

    /// <summary>
    /// (UI系统使用)获取指定槽位的卡牌信息
    /// </summary>
    /// <param name="index">下标</param>
    /// <returns>卡牌的sprite</returns>
    public PlantsSelected GetSelectPlant(int index) => selected[index];

    /// <summary>
    /// 添加一个植物到植物卡槽
    /// </summary>
    /// <param name="plantName"></param>
    public void AddSelectPlant(string plantName)
    {
        selected.Add(new PlantsSelected(PlantPrefabSerializer.Instance.GetPlantData(plantName)));
    }
    
    /// <summary>
    /// 能量管理模块
    /// </summary>
    public EnergyMonitor EnergyMonitor { get; private set; } = new EnergyMonitor();



    /// <summary>
    /// 能否放置这个下标的植物
    /// </summary>
    /// <param name="index">植物下标</param>
    /// <returns>是否出现异常</returns>
    public PlantAddException CanPlacePlant(int index)
    {
        PlantsSelected plant = selected[index];
        if (plant.CooltimePercent > 0)
            return new PlantAddException.NotCooledDownYet("植物还在冷却！");
        else if (plant.Data.EnergyCost > EnergyMonitor.Energy)
            return new PlantAddException.NotEnoughEnergy("能量不足！");
        return null;

    }
    /// <summary>
    /// （给UI系统使用）
    /// 在指定位置处放置一个植物
    /// </summary>
    /// <param name="selectIndex">选择的植物编号</param>
    /// <param name="worldPos">指定位置的像素坐标</param>
    /// <exception cref="PlantAddException.NotEnoughEnergy">能量不足</exception>
    /// <exception cref="PlantAddException.NotCooledDownYet">尚未冷却</exception>
    /// <exception cref="System.IndexOutOfRangeException">位置不合法</exception>
    public void PlacePlant(int selectIndex,Vector3 worldPos)
    {
        PlantAddException exception = CanPlacePlant(selectIndex);
        if (exception == null)//没有出错
        {
            Vector2Int gridPos = LevelData.WorldToGrid(worldPos, Level.transform.position);
            IPlantData selectedData = selected[selectIndex].Data;
            IPlantData newPlantData = PlantPrefabSerializer.Instance.GetPlantData(selectedData.PlantName);
            plantsController.AddPlant(ref newPlantData, gridPos);
            EnergyMonitor.RemoveEnergy(selectedData.EnergyCost);//移除能量
            selected[selectIndex].StartCoolTime();
        }
        else
            throw exception;
    }

    /// <summary>
    /// （给UI系统使用）
    /// 在指定位置处移除一个植物
    /// </summary>
    /// <param name="worldPos">指定位置的世界位置</param>
    /// <exception cref="System.IndexOutOfRangeException">位置不合法</exception>
    public void RemovePlant(Vector3 worldPos)
    {
        //TODO:实现逻辑判断
        plantsController.RemoveOnePlant(WorldToGrid(worldPos));
    }

    /// <summary>
    /// 将世界坐标转换为格子坐标
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector2Int WorldToGrid(Vector3 worldPos) => LevelData.WorldToGrid(worldPos, level.transform.position);
    /// <summary>
    /// 将格子坐标转换为世界坐标
    /// </summary>
    /// <param name="gridPos"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    public Vector3 GridToWorld(Vector2Int gridPos, GridPosition offset) => LevelData.GridToWorld(gridPos, offset, level.transform.position);


    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        updater.Update();
    }
    /// <summary>
    /// 游戏是否正在运行
    /// </summary>
    public bool IsGameStarted => gameObject.activeSelf;
    /// <summary>
    /// 启动游戏
    /// </summary>
    public void StartGame()
    {
        gameObject.SetActive(true);

        //添加植物
        AddSelectPlant("Mona");
        AddSelectPlant("Yanfei");
        AddSelectPlant("Lisa");
        
        //初始化模块
        plantsController = new PlantsController(LevelData);
        flyerController = new FlyersController();
        monsterController = new MonstersController();
        updater = new Updater(monsterController, LevelData.MonsterList);

        level.GetComponent<SpriteRenderer>().sprite = LevelData.Sprite;//设置关卡图片

        //展示卡槽图片
        UIManager.Instance.ShowPanel<PlantsCardPanel>("PlantsCardPanel",UIManager.UILayer.Mid,(panel)=>panel.SetPlotCount(selected.Count));

        //测试：直接添加能量
        EnergyMonitor.AddEnergy(10000);
        for (int i = 200; i < 1600;i +=100)
        {
            EnergyMonitor.InstantiateEnergy(new Vector2Int(i, 540), EnergyType.Big);
        }
    }
    /// <summary>
    /// 结束游戏
    /// </summary>
    public void EndGame()
    {
        gameObject.SetActive(false);
    }


}
