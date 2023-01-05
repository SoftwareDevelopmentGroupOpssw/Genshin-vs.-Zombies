using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 选择关卡界面
/// </summary>
public class LevelsPanel : BasePanel
{
    public LevelDatabase sprites;
    public GridLayoutGroup content;
    public GameObject levelPlot;
    private List<Button> levelButtons = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void BeforeShow()
    {
        foreach(var item in LevelSpriteSerializer.Instance)
        {
            GameObject plot = Instantiate(levelPlot, content.transform);
            plot.GetComponent<Image>().sprite = item.Value.Sprite;
            plot.name = item.Key; //改名
            plot.GetComponent<LevelPlot>().SetLevelName(item.Key); //显示关卡的名字
            Button btn = plot.GetComponent<Button>(); //为按钮添加监听
            btn.onClick.AddListener(OnButtonClicked);
            levelButtons.Add(btn);
        }
        GetControl<Button>("BackBtn").onClick.AddListener(BackTitle);
    }
    protected override void BeforeHide()
    {
        foreach(Button btn in levelButtons)
        {
            btn.onClick.RemoveAllListeners();
            Destroy(btn.gameObject);
        }
        levelButtons.Clear();

        GetControl<Button>("BackBtn").onClick.RemoveListener(BackTitle);
    }
    void BackTitle()
    {
        AudioManager.Instance.PlayEffectAudio("buttonclick");
        Hide();
        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
    }
    void OnButtonClicked()
    {
        AudioManager.Instance.PlayEffectAudio("buttonclick");
        Hide();
        string levelName = EventSystem.current.currentSelectedGameObject.name;
        GameController.Instance.LevelData = LevelSpriteSerializer.Instance.GetLevel(levelName);//设置关卡

        GameController.Instance.StartGame();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
