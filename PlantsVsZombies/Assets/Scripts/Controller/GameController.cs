using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ��Ϸ������
/// ����������ʱ��Ϊ����Ľű��Զ�����ӵ������У�ֻ��һ��ʵ��
/// ����Ϸ������ʱΪActive����Ϸ���ڽ�����ʱΪDisactive
/// </summary>
public class GameController : MonoBehaviour
{
    private static GameController instance;
    /// <summary>
    /// ����
    /// </summary>
    public static GameController Instance => instance;
    
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
    /// ��������ģ��
    /// </summary>
    public EnergyMonitor EnergyMonitor { get; private set; } = new EnergyMonitor();

    /// <summary>
    /// ����ģ�飬���𳡾��ϵ�֡����
    /// </summary>
    private Updater updater;

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
    /// <param name="pixelPos">ָ��λ�õ���������</param>
    /// <exception cref="PlantAddException.NotEnoughEnergy">��������</exception>
    /// <exception cref="PlantAddException.NotCooledDownYet">��δ��ȴ</exception>
    public void PlacePlant(int selectIndex,Vector2Int pixelPos)
    {
        PlantAddException exception = CanPlacePlant(selectIndex);
        if (exception == null)//û�г���
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector2(pixelPos.x, pixelPos.y));
            Vector2Int gridPos = LevelData.WorldToGrid(worldPos, Level.transform.position);
            IPlantData selectedData = selected[selectIndex].Data;
            IPlantData newPlantData = PlantSerializer.Instance.GetPlantData(selectedData.PlantName);
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
    /// <param name="pixelPos">ָ��λ�õ�����λ��</param>
    /// <returns>�Ƴ�����Ƿ�ɹ�</returns>
    public void RemovePlant(Vector2Int pixelPos)
    {
        //TODO:ʵ���߼��ж�
        plantsController.RemoveOnePlant(pixelPos);
    }
    
    /// <summary>
    /// ��ӷ�����
    /// </summary>
    /// <param name="data">����������</param>
    /// <param name="worldPos">��������ֵ���������</param>
    /// <returns>���������</returns>
    public Flyer AddFlyer(IFlyerData data, Vector3 worldPos) => flyerController.AddFlyer(data, worldPos);


    /// <summary>
    /// ��Ϸ����ǰ����߼�
    /// </summary>
    void OnEnable()
    {
        
    }
    /// <summary>
    /// ��Ϸ����ʱ����߼�
    /// </summary>
    void OnDisable()
    {

    }
    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
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
        gameObject.SetActive(true);

        selected.Add(new PlantsSelected(PlantSerializer.Instance.GetPlantData("Mona")));
        selected.Add(new PlantsSelected(PlantSerializer.Instance.GetPlantData("Yanfei")));
        
        plantsController = new PlantsController(LevelData);
        flyerController = new FlyersController();
        monsterController = new MonstersController(LevelData);
        
        updater = new Updater(plantsController, monsterController);

        level.GetComponent<SpriteRenderer>().sprite = LevelData.Sprite;//���ùؿ�ͼƬ

        //չʾ����ͼƬ
        UIManager.Instance.ShowPanel<PlantsCardPanel>("PlantsCardPanel",UIManager.UILayer.Mid,(panel)=>panel.SetPlotCount(selected.Count));

        EnergyMonitor.AddEnergy(1000);
        for (int i = 200; i < 1600;i +=100)
        {
            EnergyMonitor.InstantiateEnergy(new Vector2Int(i, 540), EnergyType.Big);
        }
    }
    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void EndGame()
    {
        gameObject.SetActive(false);
    }


}
