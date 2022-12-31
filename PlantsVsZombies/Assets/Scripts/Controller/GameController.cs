using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ��Ϸ������
/// ����������ʱ��Ϊ����Ľű��Զ�����ӵ������У�ֻ��һ��ʵ��
/// ����Ϸ������ʱΪActive����Ϸ���ڽ�����ʱΪDisactive
/// </summary>
public sealed partial class GameController : MonoBehaviour
{
    private static GameController instance;
    /// <summary>
    /// ����
    /// </summary>
    public static GameController Instance => instance;
    
    /// <summary>
    /// ����ģ�飬���𳡾��ϵ�֡����
    /// </summary>
    private Updater updater;
    
    /// <summary>
    /// �ؿ���Ϣ
    /// </summary>
    public ILevelData LevelData { get; set; }//��ѡ��ؿ����ʱ�����ùؿ�

    [SerializeField]
    private GameObject level;
    /// <summary>
    /// װ���ŵ�ͼSprite����Ϸ����
    /// </summary>
    public GameObject Level { get => level; }

    private PlantsController plantsController;
    /// <summary>
    /// ����ֲ��Ŀ�����
    /// </summary>
    public PlantsController PlantsController => plantsController;

    private FlyersController flyerController;
    /// <summary>
    /// ���з�����Ŀ�����
    /// </summary>
    public FlyersController FlyersController => flyerController;

    private MonstersController monsterController;
    /// <summary>
    /// ����ħ��Ŀ�����
    /// </summary>
    public MonstersController MonstersController => monsterController;
    
    /// <summary>
    /// ��ѡ���ֲ��
    /// </summary>
    private List<PlantsSelected> selected = new List<PlantsSelected>();

    /// <summary>
    /// (UIϵͳʹ��)��ȡָ����λ�Ŀ�����Ϣ
    /// </summary>
    /// <param name="index">�±�</param>
    /// <returns>���Ƶ�sprite</returns>
    public PlantsSelected GetSelectPlant(int index) => selected[index];

    /// <summary>
    /// ���һ��ֲ�ﵽֲ�￨��
    /// </summary>
    /// <param name="plantName"></param>
    public void AddSelectPlant(string plantName)
    {
        selected.Add(new PlantsSelected(PlantPrefabSerializer.Instance.GetPlantData(plantName)));
    }
    
    /// <summary>
    /// ��������ģ��
    /// </summary>
    public EnergyMonitor EnergyMonitor { get; private set; } = new EnergyMonitor();



    /// <summary>
    /// �ܷ��������±��ֲ��
    /// </summary>
    /// <param name="index">ֲ���±�</param>
    /// <returns>�Ƿ�����쳣</returns>
    public PlantAddException CanPlacePlant(int index)
    {
        PlantsSelected plant = selected[index];
        if (plant.CooltimePercent > 0)
            return new PlantAddException.NotCooledDownYet("ֲ�ﻹ����ȴ��");
        else if (plant.Data.EnergyCost > EnergyMonitor.Energy)
            return new PlantAddException.NotEnoughEnergy("�������㣡");
        return null;

    }
    /// <summary>
    /// ����UIϵͳʹ�ã�
    /// ��ָ��λ�ô�����һ��ֲ��
    /// </summary>
    /// <param name="selectIndex">ѡ���ֲ����</param>
    /// <param name="worldPos">ָ��λ�õ���������</param>
    /// <exception cref="PlantAddException.NotEnoughEnergy">��������</exception>
    /// <exception cref="PlantAddException.NotCooledDownYet">��δ��ȴ</exception>
    /// <exception cref="System.IndexOutOfRangeException">λ�ò��Ϸ�</exception>
    public void PlacePlant(int selectIndex,Vector3 worldPos)
    {
        PlantAddException exception = CanPlacePlant(selectIndex);
        if (exception == null)//û�г���
        {
            Vector2Int gridPos = LevelData.WorldToGrid(worldPos, Level.transform.position);
            if (gridPos == new Vector2(-1, -1))
                throw new System.IndexOutOfRangeException("Not available location");
            IPlantData selectedData = selected[selectIndex].Data;
            IPlantData newPlantData = PlantPrefabSerializer.Instance.GetPlantData(selectedData.PlantName);
            plantsController.AddPlant(ref newPlantData, gridPos);
            EnergyMonitor.RemoveEnergy(selectedData.EnergyCost);//�Ƴ�����
            selected[selectIndex].StartCoolTime();
        }
        else
            throw exception;
    }

    /// <summary>
    /// ����UIϵͳʹ�ã�
    /// ��ָ��λ�ô��Ƴ�һ��ֲ��
    /// </summary>
    /// <param name="worldPos">ָ��λ�õ�����λ��</param>
    /// <exception cref="System.IndexOutOfRangeException">λ�ò��Ϸ�</exception>
    public void RemovePlant(Vector3 worldPos)
    {
        plantsController.RemoveOnePlant(WorldToGrid(worldPos));
    }

    /// <summary>
    /// ����������ת��Ϊ��������
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public Vector2Int WorldToGrid(Vector3 worldPos) => LevelData.WorldToGrid(worldPos, level.transform.position);
    /// <summary>
    /// ����������ת��Ϊ��������
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
    /// ��Ϸ�Ƿ���������
    /// </summary>
    public bool IsGameStarted => gameObject.activeSelf;
    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void StartGame()
    {
        if(LevelData == null)
        {
            Debug.LogError("Level not selected. The game cannot be started");
            return;
        }

        System.GC.Collect();
        gameObject.SetActive(true);

        //���ֲ��
        AddSelectPlant("Mona");
        AddSelectPlant("Yanfei");
        AddSelectPlant("Lisa");
        AddSelectPlant("Nahida");
        AddSelectPlant("Sucrose");
        
        //��ʼ��ģ��
        plantsController = new PlantsController();
        flyerController = new FlyersController();
        monsterController = new MonstersController();

        updater = new Updater();

        level.GetComponent<SpriteRenderer>().sprite = LevelData.Sprite;//���ùؿ�ͼƬ

        //չʾ����ͼƬ
        UIManager.Instance.ShowPanel<PlantsCardPanel>("PlantsCardPanel",UIManager.UILayer.Mid,(panel)=>panel.SetPlotCount(selected.Count));

        //���ԣ�ֱ���������
        EnergyMonitor.AddEnergy(10000);
        for (int i = 200; i < 1600;i +=100)
        {
            EnergyMonitor.InstantiateEnergy(new Vector2Int(i, 540), EnergyType.Big);
        }
    }
    /// <summary>
    /// ��Ϸ�Ƿ���ͣ
    /// </summary>
    public bool IsPaused { get; private set; } = false;
    /// <summary>
    /// ��ͣ��Ϸ
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
        EnergyMonitor.ForeachEnergy((energy) =>
        {
            energy.enabled = false;
        });
        IsPaused = true;
    }
    /// <summary>
    /// ������Ϸ
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
        EnergyMonitor.ForeachEnergy((energy) =>
        {
            energy.enabled = true;
        });
        IsPaused = false;
    }
    /// <summary>
    /// ��ʾ��Ϸ���
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
        selected.Clear();
        IsPaused = false;


        EnergyMonitor.Energy = 0;
        EnergyMonitor.Clear();

        UIManager.Instance.HidePanel("PlantsCardPanel");
        gameObject.SetActive(false);
    }
}
