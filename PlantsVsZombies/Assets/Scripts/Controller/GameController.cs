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
    public GameObject Level { get; }
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
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
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

    public bool IsGameStarted => gameObject.activeSelf;
    public void StartGame()
    {
        gameObject.SetActive(true);
    }
    public void EndGame()
    {
        gameObject.SetActive(false);
    }
}
