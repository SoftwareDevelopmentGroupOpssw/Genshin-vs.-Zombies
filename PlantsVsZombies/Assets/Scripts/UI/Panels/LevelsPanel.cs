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
            plot.name = item.Key;
            Button btn = plot.GetComponent<Button>();
            btn.onClick.AddListener(OnButtonClicked);
            levelButtons.Add(btn);
        }
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
