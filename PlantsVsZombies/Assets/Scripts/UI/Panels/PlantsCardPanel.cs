using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ֲ�￨�۽���
/// </summary>
public class PlantsCardPanel : BasePanel
{

    private int selected = -1;// ����-1˵��û��ѡ��ֲ�Ϊ����ֵ��˵��ѡ�е�λ��ֲ��

    private GameController controller;
    /// <summary>
    /// ���ɵĿ���
    /// </summary>
    private List<Plot> plotList = new List<Plot>();

    [Header("ʹ�õĿ���")]
    public GameObject plot;
    [Header("ʹ�õĿ�������")]
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
            //���ɿ���
            GameObject obj = Instantiate(plot, area.transform);
            Image objImage = obj.GetComponent<Image>();
            objImage.sprite = GameController.Instance.GetSelectPlant(i).Data.CardSprite;//���ÿ���ͼƬ
            obj.name = i.ToString();
            obj.GetComponent<Button>().onClick.AddListener(OnPlotClicked);
            //���ɿ�������
            GameObject maskObj = Instantiate(mask, obj.transform);
            Image maskImage = maskObj.GetComponent<Image>();
            (maskImage.transform as RectTransform).sizeDelta = area.cellSize;//�����ֵĴ�С���ó��뿨�ƴ�Сһ��
            //��Ӵ���
            plotList.Add(new Plot() { PlotObj = objImage,Mask = maskImage});
        }
    }

    private void OnPlotClicked()//���ʱ����
    {
        int num = System.Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name);
        if (controller.CanPlacePlant(num) == null)//û�д�����
            selected = num;
    }

    private void OnShovelClicked()
    {
        selected = plotList.Count;//�����8��plot����ѡ��plotʱ��ΧΪ0-7����ʱ����Ϊ8����ʾΪ����
    }


    private GameObject real;//ѡ��ʱ�����ֵ�ʵ��ֲ��
    private GameObject unreal;//ѡ��ʱ�����ֵ�����ֲ��

    /// <summary>
    /// ��ѡ��ֲ��ʱ������ʵ������Ч��
    /// </summary>
    private void DisplayImageOnSelecting()
    {
        PlantsSelected selectPlant = GameController.Instance.GetSelectPlant(selected);
        //����ʵ�������
        if (real == null)//������ʵ���򴴽�
        {
            real = Instantiate(selectPlant.Data.OriginalReference);
            real.GetComponent<Plant>().enabled = false;//ֻ��һ���񣬲���Ҫ�����߼�����
        }
        if(unreal == null)//�����������򴴽�
        {
            unreal = Instantiate(selectPlant.Data.OriginalReference);
            unreal.GetComponent<Plant>().enabled = false;//ֻ��һ���񣬲���Ҫ�����߼�����
        }
        //��������͸����
        SpriteRenderer sprite = unreal.GetComponent<SpriteRenderer>();
        Color c = sprite.color; c.a = 0.5f; sprite.color = c;
        //��ȡ��Ϸ�����ڼ��صĹؿ�����λ�ú͹ؿ�����
        ILevelData levelData = GameController.Instance.LevelData;
        Vector3 levelPos = GameController.Instance.Level.transform.position;
        //��ʼ����ʵ��������ʾ����
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        real.transform.position = new Vector3(worldPos.x, worldPos.y, 0);//ʵ������

        Vector2Int gridPos = levelData.WorldToGrid(Camera.main.ScreenToWorldPoint(Input.mousePosition), levelPos);
        if (gridPos != new Vector2Int(-1, -1))//�ڸ����������ʾ����
        {
            unreal.SetActive(true);
            unreal.transform.position = levelData.GridToWorld(gridPos, GridPosition.Middle, levelPos);
        }
        else
            unreal.SetActive(false);
    }
    /// <summary>
    /// ˢ��ֲ����Ƿ����
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
        else//�ݻ�ʵ�������
        {
            Destroy(real); real = null;
            Destroy(unreal); unreal = null;
        }

        //��ֲ�ﱻѡ�У�������ʾΪ��ɫ��û�б�ң�������������ʱ�����в���
        if (selected != -1 && plotList[selected].PlotObj.color == Color.white && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2Int pixelPos = new Vector2Int((int)mousePos.x, (int)mousePos.y);
            if (selected != plotList.Count)//���ǲ���
            {
                controller.PlacePlant(selected, pixelPos);
                selected = -1;
                Debug.Log("����ֲ����" + pixelPos.ToString());
            }
            else
            {
                controller.RemovePlant(pixelPos);
                selected = -1;
                Debug.Log("�Ƴ�ֲ��" + pixelPos.ToString());
            }
        }
    }
    
    /// <summary>
    /// ����������Ϣ
    /// </summary>
    struct Plot
    {
        /// <summary>
        /// ���ɵĿ��۶���
        /// </summary>
        public Image PlotObj;
        /// <summary>
        /// �����ж���ȴʱ�������
        /// </summary>
        public Image Mask;
    }
}
