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
    private List<GameObject> plotList = new List<GameObject>();

    public GameObject plot;
    public GridLayoutGroup area;
    public Transform energyLocation;
    private void OnEnergyChanged(int value) => GetControl<Text>("Energy").text = value.ToString();
    protected override void BeforeShow()
    {
        controller = GameController.Instance;
        if (controller.gameObject.activeSelf)
        {
            controller.Energy.OnValueChanged += OnEnergyChanged;

            GetControl<Button>("ShovelBtn").onClick.AddListener(OnShovelClicked);
        }
        else
            Debug.LogError("The game is not started.");
    }
    protected override void BeforeHide()
    {
        controller.Energy.OnValueChanged -= OnEnergyChanged;
        GetControl<Button>("ShovelBtn").onClick.RemoveAllListeners();

        SetPlotCount(0);
    }
    public void SetPlotCount(int count)
    {
        if (count == plotList.Count)
            return;

        foreach(GameObject plot in plotList)
        {
            Destroy(plot);
        }
        plotList.Clear();

        RectTransform rect = transform as RectTransform;
        int basicWidth = 160;
        rect.sizeDelta = new Vector2(basicWidth + area.cellSize.x * count + area.padding.left + area.padding.right + area.spacing.x * (count - 1),rect.sizeDelta.y);
        for(int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(plot, area.transform);
            obj.GetComponent<Image>().sprite = GameController.Instance.GetSelectPlant(i).Data.CardSprite;//���ÿ���ͼƬ
            obj.name = i.ToString();
            obj.GetComponent<Button>().onClick.AddListener(OnPlotClicked);
            plotList.Add(obj);
        }
    }
    private void OnPlotClicked()
    {
        selected = System.Convert.ToInt32(EventSystem.current.currentSelectedGameObject.name);
    }
    private void OnShovelClicked()
    {
        selected = plotList.Count;//�����8��plot����ѡ��plotʱ��ΧΪ0-7����ʱ����Ϊ8����ʾΪ����
    }


    private GameObject real;//ѡ��ʱ�����ֵ�ʵ��ֲ��
    private GameObject unreal;//ѡ��ʱ�����ֵ�����ֲ��

    private void DisplayImageOnSelecting()//��ѡ��ֲ��ʱ������ʵ������Ч��
    {
        PlantsSelected selectPlant = GameController.Instance.GetSelectPlant(selected);
        //����ʵ�������
        if(real == null)//������ʵ���򴴽�
            real = Instantiate(selectPlant.Data.OriginalReference);
        if(unreal == null)//�����������򴴽�
            unreal = Instantiate(selectPlant.Data.OriginalReference);
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

    private void Update()
    {
        if(selected != -1)
        {
            DisplayImageOnSelecting();
        }
        else//�ݻ�ʵ�������
        {
            Destroy(real); real = null;
            Destroy(unreal); unreal = null;
        }

        if (selected != -1 && Input.GetMouseButtonDown(0))//����ֲ�ﱻѡ�����ٴΰ������ʱ���ж��߼�
        {
            Vector3 mousePos = Input.mousePosition;
            Vector2Int pixelPos = new Vector2Int((int)mousePos.x, (int)mousePos.y);
            if (selected != plotList.Count)//���ǲ���
            {
                if (controller.TryPlacePlant(selected, pixelPos))
                {
                    selected = -1;
                    Debug.Log("����ֲ����" +pixelPos.ToString());
                }
            }
            else
            {
                if (controller.TryRemovePlant(pixelPos))
                {
                    selected = -1;
                    Debug.Log("�Ƴ�ֲ��" + pixelPos.ToString());
                }
            }
        }
    }
}
