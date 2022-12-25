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
    private void Update()
    {
        if(selected != -1 && Input.GetMouseButtonDown(0))//����ֲ�ﱻѡ�����ٴΰ������ʱ���ж��߼�
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
