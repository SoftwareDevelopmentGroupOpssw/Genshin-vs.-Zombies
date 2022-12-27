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

    /// <summary>
    /// ����ֲ��Ŀ�����
    /// </summary>
    private PlantsController plantsController;
    /// <summary>
    /// ���з�����Ŀ�����
    /// </summary>
    private FlyersController flyerController;

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
    /// ��ȡָ����λ�Ŀ��Ʋ������Ϣ
    /// </summary>
    /// <param name="index">�±�</param>
    /// <returns></returns>
    public PlantsSelected GetSelectPlant(int index) => selected[index];

    
    /// <summary>
    /// ��������ģ��
    /// </summary>
    public EnergyMonitor Energy { get; private set; } = new EnergyMonitor();

    /// <summary>
    /// ����ģ�飬���𳡾��ϵ�֡����
    /// </summary>
    private Updater updater;

    /// <summary>
    /// ����UIϵͳʹ�ã�
    /// ��ָ��λ�ô����Է���һ��ֲ��
    /// </summary>
    /// <param name="selectIndex">ѡ���ֲ����</param>
    /// <param name="pixelPos">ָ��λ�õ���������</param>
    /// <returns>���ý���ɹ����</returns>
    public bool TryPlacePlant(int selectIndex,Vector2Int pixelPos)
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector2(pixelPos.x,pixelPos.y));
        Vector2Int gridPos = LevelData.WorldToGrid(worldPos, Level.transform.position);
        Plant plant;
        return plantsController.TryAddPlant(selected[selectIndex].Data, gridPos, out plant);
    }
    
    /// <summary>
    /// ����UIϵͳʹ�ã�
    /// ��ָ��λ�ô������Ƴ�һ��ֲ��
    /// </summary>
    /// <param name="pixelPos">ָ��λ�õ�����λ��</param>
    /// <returns>�Ƴ�����Ƿ�ɹ�</returns>
    public bool TryRemovePlant(Vector2Int pixelPos)
    {
        //TODO:ʵ���߼��ж�
        return true;
    }
    
    /// <summary>
    /// ��ӷ�����
    /// </summary>
    /// <param name="data">����������</param>
    /// <param name="pixelPos">��������ֵ���������</param>
    /// <returns>���������</returns>
    public Flyer AddFlyer(IFlyerData data, Vector2Int pixelPos) => flyerController.AddFlyer(data, pixelPos);


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
        
        plantsController = new PlantsController(LevelData);
        flyerController = new FlyersController();
        monsterController = new MonstersController(LevelData);
        
        updater = new Updater(plantsController, monsterController);

        level.GetComponent<SpriteRenderer>().sprite = LevelData.Sprite;//���ùؿ�ͼƬ

        //չʾ����ͼƬ
        UIManager.Instance.ShowPanel<PlantsCardPanel>("PlantsCardPanel",UIManager.UILayer.Mid,(panel)=>panel.SetPlotCount(selected.Count));

        for (int i = 200; i < 800; i += 100)
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
