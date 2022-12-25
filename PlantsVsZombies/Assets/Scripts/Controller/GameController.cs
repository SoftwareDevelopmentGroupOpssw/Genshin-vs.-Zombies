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
    /// <summary>
    /// װ���ŵ�ͼSprite����Ϸ����
    /// </summary>
    public GameObject Level { get; private set; }
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
    /// ��������ģ��
    /// </summary>
    public EnergyMonitor Energy { get; private set; } = new EnergyMonitor();


    /// <summary>
    /// ����UIϵͳʹ�ã�
    /// ��ָ��λ�ô����Է���һ��ֲ��
    /// </summary>
    /// <param name="selectIndex">ѡ���ֲ����</param>
    /// <param name="pixelPos">ָ��λ�õ���������</param>
    /// <returns>���ý���ɹ����</returns>
    public bool TryPlacePlant(int selectIndex,Vector2Int pixelPos)
    {
        //TODO:ʵ���߼��ж�
        return true;
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
        plantsController = new PlantsController(LevelData);
        flyerController = new FlyersController();
        monsterController = new MonstersController(LevelData);
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        UIManager.Instance.ShowPanel<LevelBackground>
            (
                "LevelBackground",
                UIManager.UILayer.Top,
                (panel) =>
                {
                    panel.Sprite = LevelData.Sprite;
                    Level = panel.gameObject;
                }
            );
        UIManager.Instance.ShowPanel<PlantsCardPanel>("PlantsCardPanel",UIManager.UILayer.Mid,(panel)=>panel.SetPlotCount(8));

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
