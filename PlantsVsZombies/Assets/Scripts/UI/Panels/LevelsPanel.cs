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
    public LevelSprites sprites;
    public GridLayoutGroup content;
    public GameObject levelPlot;
    private List<Button> levelButtons = new List<Button>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void BeforeShow()
    {
        sprites.Foreach((level) =>
        {
            GameObject plot = Instantiate(levelPlot, content.transform);
            plot.GetComponent<Image>().sprite = level.Sprite;
            plot.name = level.Name;
            Button btn = plot.GetComponent<Button>();
            btn.onClick.AddListener(OnButtonClicked);
            levelButtons.Add(btn);
        });
    }
    protected override void BeforeHide()
    {
        foreach(Button btn in levelButtons)
        {
            btn.onClick.RemoveAllListeners();
            Destroy(btn.gameObject);
        }
        levelButtons.Clear();
    }
    void OnButtonClicked()
    {
        string levelName = EventSystem.current.currentSelectedGameObject.name;
        //GameController.Instance.LevelData = LevelSerializer.GetLevel(levelName);//设置关卡
        GameController.Instance.LevelData = new TestLevel(sprites.SearchSprite(levelName));//测试用

        GameController.Instance.StartGame();
        Hide();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
