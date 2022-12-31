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
    public GridLayoutGroup area;//������ʾ����
    public Transform energyLocation;//������ʶ��λ�ã����ռ����������������λ�ã�
    /// <summary>
    /// ���������ı䣬�ڸı�ʱ�޸���ʾֵ
    /// </summary>
    /// <param name="value"></param>
    private void OnEnergyChanged(int value) => GetControl<Text>("Energy").text = value.ToString();
    protected override void BeforeShow()
    {
        controller = GameController.Instance;
        if (controller.gameObject.activeSelf)
        {
            controller.EnergyMonitor.OnValueChanged += OnEnergyChanged;//��Ӽ���

            GetControl<Button>("ShovelBtn").onClick.AddListener(OnShovelClicked);//��Ӳ��Ӱ�ť����
        }
        else
            Debug.LogError("The game is not started.");
    }
    protected override void BeforeHide()
    {
        controller.EnergyMonitor.OnValueChanged -= OnEnergyChanged;//�Ƴ�����
        GetControl<Button>("ShovelBtn").onClick.RemoveAllListeners();//�Ƴ����Ӱ�ť����

        SetPlotCount(0);
    }
    /// <summary>
    /// ���ÿ��۵�����
    /// </summary>
    /// <param name="count">����</param>
    public void SetPlotCount(int count)
    {
        if (count == plotList.Count)
            return;
        //�Ƴ�֮ǰ���ɵĿ���
        foreach(Plot plot in plotList)
        {
            Destroy(plot.PlotObj.gameObject);
            Destroy(plot.Mask.gameObject);
        }
        plotList.Clear();
        //��ʼ���ɿ���
        RectTransform rect = transform as RectTransform;
        int basicWidth = 160;
        rect.sizeDelta = new Vector2(basicWidth + area.cellSize.x * count + area.padding.left + area.padding.right + area.spacing.x * (count - 1),rect.sizeDelta.y);
        for(int i = 0; i < count; i++)
        {
            //���ɿ���
            GameObject obj = Instantiate(plot, area.transform);
            CardPlot plotObj = obj.GetComponent<CardPlot>();
            IPlantData data = GameController.Instance.GetSelectPlant(i).Data;//ֲ������
            plotObj.Sprite = (data.CardSprite);//���ÿ���ͼƬ
            plotObj.SetEnergyCost(data.EnergyCost);
            obj.name = i.ToString();
            obj.GetComponent<Button>().onClick.AddListener(OnPlotClicked);
            //���ɿ�������
            GameObject maskObj = Instantiate(mask, obj.transform);
            Image maskImage = maskObj.GetComponent<Image>();
            (maskImage.transform as RectTransform).sizeDelta = area.cellSize;//�����ֵĴ�С���ó��뿨�ƴ�Сһ��
            //��Ӵ���
            plotList.Add(new Plot() { PlotObj = plotObj, Mask = maskImage});
        }
    }
    /// <summary>
    /// ��ť�����ʱ����
    /// </summary>
    private void OnPlotClicked()
    {
        int num = System.Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name);
        if (controller.CanPlacePlant(num) == null)//û�д�����
        {
            if (selected != -1)
                DestroyImages();//�ݻ�֮ǰ������ʵ������
            selected = num;
        }
    }
    /// <summary>
    /// ���ӱ����ʱ����
    /// </summary>
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
        if(selected != plotList.Count)//ѡ������岻�ǲ���
        {
            PlantsSelected selectPlant = GameController.Instance.GetSelectPlant(selected);
            //����ʵ�������
            if (real == null)//������ʵ���򴴽�
            {
                real = Instantiate(selectPlant.Data.OriginalReference);
                real.GetComponent<Plant>().enabled = false;//ֻ��һ���񣬲���Ҫ�����߼�����
            }
            if (unreal == null)//�����������򴴽�
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
        else//ѡ��������ǲ���
        {
            Button btn = GetControl<Button>("ShovelBtn");
            RectTransform rect = btn.transform as RectTransform;
            Vector3 worldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle
                (
                    UIManager.Instance.Canvas.transform as RectTransform,
                    Input.mousePosition,
                    null,
                    out worldPos
                );
            rect.position = worldPos;
        }
        
    }
    /// <summary>
    /// ɾ��������ʵ������Ч��
    /// </summary>
    private void DestroyImages()
    {
        if(real != null)
        {
            Destroy(real); real = null;
        }
        if(unreal != null)
        {
            Destroy(unreal); unreal = null;
        }
        RectTransform btnTransform = GetControl<Button>("ShovelBtn").transform as RectTransform;
        btnTransform.anchoredPosition = Vector2.zero;//���ӻص�ԭλ
    }
    /// <summary>
    /// ˢ��ֲ����Ƿ����
    /// </summary>
    private void RefreshAvailableState()
    {
        for(int i = 0; i < plotList.Count; i++)
        {
            PlantAddException exception = controller.CanPlacePlant(i);
            if(exception == null)//û�з�������
            {
                plotList[i].PlotObj.SpriteColor = Color.white;
                plotList[i].Mask.fillAmount = 0;
                plotList[i].Available = true;
            }
            else
            {
                plotList[i].PlotObj.SpriteColor = Color.grey;
                if (exception is PlantAddException.NotCooledDownYet)
                {
                    PlantsSelected selectedPlant = controller.GetSelectPlant(i);
                    plotList[i].Mask.fillAmount = selectedPlant.CooltimePercent;
                }
                else if (exception is PlantAddException.NotEnoughEnergy)
                {
                    plotList[i].Mask.fillAmount = 0;
                }
                plotList[i].Available = false;
            }
        }
    }
    private void Update()
    {
        RefreshAvailableState();
        if (selected != -1)
            DisplayImageOnSelecting();
        else//�ݻ�ʵ�������
            DestroyImages();
        //��ֲ�ﱻѡ�У�������ʾΪ��ɫ��û�б�ң�������������ʱ�����в���
        if (selected != -1 && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            if (selected != plotList.Count && plotList[selected].Available)//���ǲ����Ҵ��ڿ���״̬
            {
                try
                {
                    controller.PlacePlant(selected, worldPos);
                    selected = -1;
                }
                catch (System.IndexOutOfRangeException) { }
            }
            else
            {
                try
                {
                    controller.RemovePlant(worldPos);
                    selected = -1;
                }
                catch (System.IndexOutOfRangeException) { }
            }
        }
        else if(selected != -1 && Input.GetMouseButtonDown(1))//��ѡ������ʱ�����Ҽ����ѡ��
        {
            selected = -1;
        }
    }
    
    /// <summary>
    /// ����������Ϣ
    /// </summary>
    class Plot
    {
        /// <summary>
        /// ���ɵĿ��۶���
        /// </summary>
        public CardPlot PlotObj;
        /// <summary>
        /// �����ж���ȴʱ�������
        /// </summary>
        public Image Mask;
        /// <summary>
        /// ��������Ƿ����
        /// </summary>
        public bool Available;
    }
}
