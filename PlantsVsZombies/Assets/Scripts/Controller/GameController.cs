using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 游戏控制器
/// 当程序运行时作为物体的脚本自动被添加到场景中，只有一个实例
/// 当游戏进行中时为Active，游戏不在进行中时为Disactive
/// </summary>
public sealed partial class GameController : MonoBehaviour
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


    private EntitiesController entitiesController;
    /// <summary>
    /// 所有实体的控制器
    /// </summary>
    public EntitiesController EntitiesController => entitiesController;
    
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

    #region UI面板接口
    /// <summary>
    /// 能否选择这个下标的植物
    /// </summary>
    /// <param name="index">植物下标</param>
    /// <param name="worldPos">试图放置的世界坐标</param>
    /// <returns>出现异常则返回异常，没有异常则返回null</returns>
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
    /// 能否在世界坐标处放置这个下标的植物
    /// </summary>
    /// <param name="index">植物下标</param>
    /// <param name="worldPos">试图放置的世界坐标</param>
    /// <returns>出现异常则返回异常，没有异常则返回null</returns>
    public PlantAddException CanPlacePlant(int index, Vector3 worldPos)
    {
        PlantsSelected plant = selected[index];
        Vector2Int gridPos = LevelData.WorldToGrid(worldPos, Level.transform.position);
        if (plant.CooltimePercent > 0)
            return new PlantAddException.NotCooledDownYet("植物还在冷却！");
        else if (plant.Data.EnergyCost > EnergyMonitor.Energy)
            return new PlantAddException.NotEnoughEnergy("能量不足！");
        else if (gridPos == new Vector2(-1, -1))
            return new PlantAddException.OutOfBorder("目标点在地图之外");
        else if (plantsController.HasPlant(gridPos)) //格点上已经有植物 则不能放置
            return new PlantAddException.SpaceOccupied("植物不能重叠！");
        return null;
    }
    /// <summary>
    /// （给UI系统使用）
    /// 在指定位置处放置一个植物
    /// </summary>
    /// <param name="selectIndex">选择的植物编号</param>
    /// <param name="worldPos">指定位置的世界坐标</param>
    /// <exception cref="PlantAddException.NotEnoughEnergy">能量不足</exception>
    /// <exception cref="PlantAddException.NotCooledDownYet">尚未冷却</exception>
    /// <exception cref="PlantAddException.SpaceOccupied">格子上已经有植物</exception>
    /// <exception cref="PlantAddException.OutOfBorder">位置不在地图格点内</exception>
    public void PlacePlant(int selectIndex,Vector3 worldPos)
    {
        PlantAddException exception = CanPlacePlant(selectIndex,worldPos);
        if (exception == null)//植物检查没有出错
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
        plantsController.RemoveOnePlant(WorldToGrid(worldPos));
    }
    #endregion
    #region 坐标转换
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
    #endregion

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsPaused && IsGameStarted)
            updater.Update();
    }

    #region 游戏控制
    /// <summary>
    /// 查看怪物预览信息
    /// </summary>
    private IEnumerator Preview()
    {
        level.GetComponent<SpriteRenderer>().sprite = LevelData.Sprite;//设置关卡图片

        float waitSecondsToPreview = 1.5f;//在镜头移动过去之前观察地图等待的秒数
        float duration = 2f;//观察的时间
        float movingSpeed = 1;//镜头移动的速度
        float pixelPerUnit = LevelData.Sprite.pixelsPerUnit;
        Vector3 rightUp = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector3 leftBot = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        float moveDistance = LevelData.Sprite.rect.width / pixelPerUnit - (rightUp.x - leftBot.x) - 0.2f; //-0.2f是防止精度问题移动过多而导致露出蓝色背景
        //产生预览僵尸图像的区域宽为movedistance 高为rightup.y - leftbot.y
        //以下两个offset限制产生预览图像的位置，和边界保持一定距离
        float borderOffsetX = 0.2f * moveDistance;
        float borderOffsetY = 0.2f * (rightUp.y - leftBot.y);
        //生成怪物预览
        IMonsterData[] monsters = LevelData.MonsterTypes;
        List<GameObject> list = new List<GameObject>();
        foreach (var data in monsters)
        {
            GameObject obj = new GameObject("TmpMonsterView");
            obj.transform.SetParent(level.transform);
            obj.AddComponent<SpriteRenderer>().sprite = data.OriginalReference.GetComponent<SpriteRenderer>().sprite;
            list.Add(obj);
            float xPos = rightUp.x + Random.Range(borderOffsetX, moveDistance - borderOffsetX);
            float yPos = Random.Range(leftBot.y + borderOffsetY, rightUp.y - borderOffsetY);
            obj.transform.position = new Vector2(xPos, yPos);
        }

        Vector3 startTransform = level.transform.position;
        yield return new WaitForSecondsRealtime(waitSecondsToPreview);
        //过去
        for(float i = 0; i < 1; i+= Time.deltaTime * movingSpeed)
        {
            level.transform.position = startTransform + Vector3.left * Mathf.Lerp(0, moveDistance, i); //关卡物体向左，摄像机相对向右
            yield return 1;
        }

        yield return new WaitForSecondsRealtime(duration);//预览时间

        //返回
        for (float i = 0; i < 1; i += Time.deltaTime * movingSpeed)
        {
            level.transform.position = startTransform + Vector3.left * Mathf.Lerp(0, moveDistance, 1-i);//关卡物体向左，摄像机相对向右
            yield return 1;
        }
        //摧毁预览的图像
        foreach(var obj in list)
        {
            Destroy(obj);
        }

        GameObject levelStartLabel = ResourceManager.Instance.Load<GameObject>("Prefabs/UI/UIElements/LVStartLabel");
        GameObject instance = Instantiate(levelStartLabel, Camera.main.transform);
        while(instance != null)
        {
            yield return 1;
        }
        
        RealStart();
    }
    /// <summary>
    /// 真正对游戏资源初始化
    /// </summary>
    private void RealStart()
    {
        if (LevelData == null)
        {
            Debug.LogError("Level not selected. The game cannot be started.");
            return;
        }

        System.GC.Collect();

        //添加植物
        AddSelectPlant("Mona");
        AddSelectPlant("Yanfei");
        AddSelectPlant("Lisa");
        AddSelectPlant("Nahida");
        AddSelectPlant("Sucrose");
        AddSelectPlant("Ningguang");
        AddSelectPlant("RaidenShogun");
        AddSelectPlant("WallNut");

        //初始化模块
        plantsController = new PlantsController();
        flyerController = new FlyersController();
        monsterController = new MonstersController();
        entitiesController = new EntitiesController();

        updater = new Updater();

        //展示卡槽图片
        UIManager.Instance.ShowPanel<PlantsCardPanel>("PlantsCardPanel", UIManager.UILayer.Mid, (panel) =>
        {
            panel.SetPlotCount(selected.Count);
            //能量设为50
            EnergyMonitor.Energy = 50;
        });

        IsGameStarted = true;


    }
    /// <summary>
    /// 游戏是否正在运行
    /// </summary>
    public bool IsGameStarted { get; private set; } = false;
    /// <summary>
    /// 启动游戏
    /// </summary>
    public void StartGame()
    {
        gameObject.SetActive(true);
        base.StartCoroutine(Preview());
    }
    /// <summary>
    /// 游戏是否被暂停
    /// </summary>
    public bool IsPaused { get; private set; } = false;
    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void Pause()
    {
        PlantsController.Foreach((plant) =>
        {
            plant.GetComponent<Animator>().enabled = false;
            plant.enabled = false;
        });
        FlyersController.Foreach((flyer) =>
        {
            flyer.GetComponent<Rigidbody2D>().Sleep();
            flyer.enabled = false;
        });
        MonstersController.Foreach((monster) =>
        {
            monster.GetComponent<Rigidbody2D>().Sleep();
            monster.GetComponent<Animator>().enabled = false;
            monster.enabled = false;
        });
        EntitiesController.Foreach((unite) =>
        {
            Rigidbody2D rigid;
            Animator animator;
            foreach(KeyValuePair<GameObject,List<GameObject>> pair in unite.ActiveLists)//pair是预制体和激活列表的键值对
            {
                if (TryGetComponent<Rigidbody2D>(out rigid))
                    rigid.Sleep();
                if (TryGetComponent<Animator>(out animator))
                    animator.enabled = false;
                //obj是激活列表里的所有物体
                foreach(var obj in pair.Value)
                {
                    //type是此种预制体所有已经监听的类
                    if (unite.ManagedBehaviours.ContainsKey(pair.Key))
                    {
                        foreach (var type in unite.ManagedBehaviours[pair.Key])
                        {
                            Behaviour behaviour = obj.GetComponent(type) as Behaviour;
                            behaviour.enabled = false;
                        }
                    }
                }
            }
        });
        IsPaused = true;
    }
    /// <summary>
    /// 继续游戏
    /// </summary>
    public void Resume()
    {
        PlantsController.Foreach((plant) =>
        {
            plant.GetComponent<Animator>().enabled = true;
            plant.enabled = true;
        });
        FlyersController.Foreach((flyer) =>
        {
            flyer.GetComponent<Rigidbody2D>().WakeUp();
            flyer.enabled = true;
        });
        MonstersController.Foreach((monster) =>
        {
            monster.GetComponent<Rigidbody2D>().WakeUp();
            monster.GetComponent<Animator>().enabled = true;
            monster.enabled = true;
        });
        EntitiesController.Foreach((unite) =>
        {
            Rigidbody2D rigid;
            Animator animator;
            foreach (KeyValuePair<GameObject, List<GameObject>> pair in unite.ActiveLists)//pair是预制体和激活列表的键值对
            {
                if (TryGetComponent<Rigidbody2D>(out rigid))
                    rigid.WakeUp();
                if (TryGetComponent<Animator>(out animator))
                    animator.enabled = true;
                //obj是激活列表里的所有物体
                foreach (var obj in pair.Value)
                {
                    //type是此种预制体所有已经监听的类
                    if (unite.ManagedBehaviours.ContainsKey(pair.Key))
                    {
                        foreach (var type in unite.ManagedBehaviours[pair.Key])
                        {
                            Behaviour behaviour = obj.GetComponent(type) as Behaviour;
                            behaviour.enabled = true;
                        }
                    }
                }
            }
        });
        IsPaused = false;
    }
    /// <summary>
    /// 显示游戏结果
    /// </summary>
    public void ShowResult(bool result)
    {
        Pause();
        if (result == true)
        {
            UIManager.Instance.ShowPanel<VictoryPanel>("VictoryPanel", UIManager.UILayer.System);
        }
        else
        {
            UIManager.Instance.ShowPanel<DefeatPanel>("DefeatPanel", UIManager.UILayer.System);
        }
    }
    public void EndGame()
    {
        plantsController.Clear();
        monsterController.Clear();
        flyerController.Clear();
        entitiesController.ClearAll();

        selected.Clear();
        IsPaused = false;
        EnergyMonitor.Energy = 0;

        UIManager.Instance.HidePanel("PlantsCardPanel");

        IsGameStarted = false;

        gameObject.SetActive(false);
    }
    #endregion
    /// <summary>
    /// 在GameController启动的协程会受到游戏是否暂停的控制
    /// </summary>
    /// <param name="enumerator"></param>
    /// <returns></returns>
    public new Coroutine StartCoroutine(IEnumerator enumerator)
    {
        IEnumerator RealCoroutine(IEnumerator arg)
        {
            arg.MoveNext();
            do
            {
                yield return arg.Current;
                while (IsPaused)
                    yield return 1;
            } while (arg.MoveNext());
        }
        return base.StartCoroutine(RealCoroutine(enumerator));
    }
}
