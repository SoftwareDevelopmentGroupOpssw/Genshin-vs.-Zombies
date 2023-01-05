using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePanel : BasePanel
{
    // Start is called before the first frame update
    protected override void BeforeShow()
    {
        System.GC.Collect();
        GetControl<Button>("Start").onClick.AddListener(StartGame);
        GetControl<Button>("Help").onClick.AddListener(OpenHelp);
        GetControl<Button>("Setting").onClick.AddListener(OpenSetting);
        GetControl<Button>("Exit").onClick.AddListener(Exit);
    }
    protected override void BeforeHide()
    {
        GetControl<Button>("Start").onClick.RemoveAllListeners();
        GetControl<Button>("Help").onClick.RemoveListener(OpenHelp);
        GetControl<Button>("Setting").onClick.RemoveAllListeners();
    }
    void StartGame()
    {
        AudioManager.Instance.PlayEffectAudio("buttonclick");
        Hide();
        UIManager.Instance.ShowPanel<LevelsPanel>("LevelsPanel");

    }
    void OpenHelp()
    {
        AudioManager.Instance.PlayEffectAudio("buttonclick");
        UIManager.Instance.ShowPanel<HelpPanel>("HelpPanel",UIManager.UILayer.Bot);
    }
    void OpenSetting()
    {
        AudioManager.Instance.PlayEffectAudio("buttonclick");
        Hide();
        UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel",UIManager.UILayer.Bot);
    }
    void Exit()
    {
        AudioManager.Instance.PlayEffectAudio("buttonclick");
        Application.Quit(0);
    }
}
