using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {

    }
    protected override void BeforeShow()
    {
        AudioManager.Instance.PauseBackgroundAudio();
        AudioManager.Instance.PlayEffectAudio("winmusic");
        GetControl<Button>("BackToSelectLevelBtn").onClick.AddListener(BackToSelectLevel);
        GetControl<Button>("TitleBtn").onClick.AddListener(ToTitle);
    }
    protected override void BeforeHide()
    {
        AudioManager.Instance.ResumeBackgroundAudio();
        GetControl<Button>("BackToSelectLevelBtn").onClick.RemoveListener(BackToSelectLevel);
        GetControl<Button>("TitleBtn").onClick.RemoveListener(ToTitle);
    }
    void ToTitle()
    {
        AudioManager.Instance.PlayEffectAudio("buttonclick");
        GameController.Instance.EndGame();
        UIManager.Instance.HidePanel("VictoryPanel");
        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
    }
    void BackToSelectLevel()
    {
        AudioManager.Instance.PlayEffectAudio("buttonclick");
        GameController.Instance.EndGame();
        UIManager.Instance.HidePanel("VictoryPanel");
        UIManager.Instance.ShowPanel<LevelsPanel>("LevelsPanel");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
