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


    private EntitiesController entitiesController;
    /// <summary>
    /// ����ʵ��Ŀ�����
    /// </summary>
    public EntitiesController EntitiesController => entitiesController;
    
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

    #region UI���ӿ�
    /// <summary>
    /// �ܷ�ѡ������±��ֲ��
    /// </summary>
    /// <param name="index">ֲ���±�</param>
    /// <param name="worldPos">��ͼ���õ���������</param>
    /// <returns>�����쳣�򷵻��쳣��û���쳣�򷵻�null</returns>
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
    /// �ܷ����������괦��������±��ֲ��
    /// </summary>
    /// <param name="index">ֲ���±�</param>
    /// <param name="worldPos">��ͼ���õ���������</param>
    /// <returns>�����쳣�򷵻��쳣��û���쳣�򷵻�null</returns>
    public PlantAddException CanPlacePlant(int index, Vector3 worldPos)
    {
        PlantsSelected plant = selected[index];
        Vector2Int gridPos = LevelData.WorldToGrid(worldPos, Level.transform.position);
        if (plant.CooltimePercent > 0)
            return new PlantAddException.NotCooledDownYet("ֲ�ﻹ����ȴ��");
        else if (plant.Data.EnergyCost > EnergyMonitor.Energy)
            return new PlantAddException.NotEnoughEnergy("�������㣡");
        else if (gridPos == new Vector2(-1, -1))
            return new PlantAddException.OutOfBorder("Ŀ����ڵ�ͼ֮��");
        else if (plantsController.HasPlant(gridPos)) //������Ѿ���ֲ�� ���ܷ���
            return new PlantAddException.SpaceOccupied("ֲ�ﲻ���ص���");
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
    /// <exception cref="PlantAddException.SpaceOccupied">�������Ѿ���ֲ��</exception>
    /// <exception cref="PlantAddException.OutOfBorder">λ�ò��ڵ�ͼ�����</exception>
    public void PlacePlant(int selectIndex,Vector3 worldPos)
    {
        PlantAddException exception = CanPlacePlant(selectIndex,worldPos);
        if (exception == null)//ֲ����û�г���
        {
            Vector2Int gridPos = LevelData.WorldToGrid(worldPos, Level.transform.position);
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
    #endregion
    #region ����ת��
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

    #region ��Ϸ����
    /// <summary>
    /// �鿴����Ԥ����Ϣ
    /// </summary>
    private IEnumerator Preview()
    {
        level.GetComponent<SpriteRenderer>().sprite = LevelData.Sprite;//���ùؿ�ͼƬ

        float waitSecondsToPreview = 1.5f;//�ھ�ͷ�ƶ���ȥ֮ǰ�۲��ͼ�ȴ�������
        float duration = 2f;//�۲��ʱ��
        float movingSpeed = 1;//��ͷ�ƶ����ٶ�
        float pixelPerUnit = LevelData.Sprite.pixelsPerUnit;
        Vector3 rightUp = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector3 leftBot = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        float moveDistance = LevelData.Sprite.rect.width / pixelPerUnit - (rightUp.x - leftBot.x) - 0.2f; //-0.2f�Ƿ�ֹ���������ƶ����������¶����ɫ����
        //����Ԥ����ʬͼ��������Ϊmovedistance ��Ϊrightup.y - leftbot.y
        //��������offset���Ʋ���Ԥ��ͼ���λ�ã��ͱ߽籣��һ������
        float borderOffsetX = 0.2f * moveDistance;
        float borderOffsetY = 0.2f * (rightUp.y - leftBot.y);
        //���ɹ���Ԥ��
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
        //��ȥ
        for(float i = 0; i < 1; i+= Time.deltaTime * movingSpeed)
        {
            level.transform.position = startTransform + Vector3.left * Mathf.Lerp(0, moveDistance, i); //�ؿ���������������������
            yield return 1;
        }

        yield return new WaitForSecondsRealtime(duration);//Ԥ��ʱ��

        //����
        for (float i = 0; i < 1; i += Time.deltaTime * movingSpeed)
        {
            level.transform.position = startTransform + Vector3.left * Mathf.Lerp(0, moveDistance, 1-i);//�ؿ���������������������
            yield return 1;
        }
        //�ݻ�Ԥ����ͼ��
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
    /// ��������Ϸ��Դ��ʼ��
    /// </summary>
    private void RealStart()
    {
        if (LevelData == null)
        {
            Debug.LogError("Level not selected. The game cannot be started.");
            return;
        }

        System.GC.Collect();

        //���ֲ��
        AddSelectPlant("Mona");
        AddSelectPlant("Yanfei");
        AddSelectPlant("Lisa");
        AddSelectPlant("Nahida");
        AddSelectPlant("Sucrose");
        AddSelectPlant("Ningguang");
        AddSelectPlant("RaidenShogun");
        AddSelectPlant("WallNut");

        //��ʼ��ģ��
        plantsController = new PlantsController();
        flyerController = new FlyersController();
        monsterController = new MonstersController();
        entitiesController = new EntitiesController();

        updater = new Updater();

        //չʾ����ͼƬ
        UIManager.Instance.ShowPanel<PlantsCardPanel>("PlantsCardPanel", UIManager.UILayer.Mid, (panel) =>
        {
            panel.SetPlotCount(selected.Count);
            //������Ϊ50
            EnergyMonitor.Energy = 50;
        });

        IsGameStarted = true;


    }
    /// <summary>
    /// ��Ϸ�Ƿ���������
    /// </summary>
    public bool IsGameStarted { get; private set; } = false;
    /// <summary>
    /// ������Ϸ
    /// </summary>
    public void StartGame()
    {
        gameObject.SetActive(true);
        base.StartCoroutine(Preview());
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
        EntitiesController.Foreach((unite) =>
        {
            Rigidbody2D rigid;
            Animator animator;
            foreach(KeyValuePair<GameObject,List<GameObject>> pair in unite.ActiveLists)//pair��Ԥ����ͼ����б�ļ�ֵ��
            {
                if (TryGetComponent<Rigidbody2D>(out rigid))
                    rigid.Sleep();
                if (TryGetComponent<Animator>(out animator))
                    animator.enabled = false;
                //obj�Ǽ����б������������
                foreach(var obj in pair.Value)
                {
                    //type�Ǵ���Ԥ���������Ѿ���������
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
        EntitiesController.Foreach((unite) =>
        {
            Rigidbody2D rigid;
            Animator animator;
            foreach (KeyValuePair<GameObject, List<GameObject>> pair in unite.ActiveLists)//pair��Ԥ����ͼ����б�ļ�ֵ��
            {
                if (TryGetComponent<Rigidbody2D>(out rigid))
                    rigid.WakeUp();
                if (TryGetComponent<Animator>(out animator))
                    animator.enabled = true;
                //obj�Ǽ����б������������
                foreach (var obj in pair.Value)
                {
                    //type�Ǵ���Ԥ���������Ѿ���������
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
    /// ��GameController������Э�̻��ܵ���Ϸ�Ƿ���ͣ�Ŀ���
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
