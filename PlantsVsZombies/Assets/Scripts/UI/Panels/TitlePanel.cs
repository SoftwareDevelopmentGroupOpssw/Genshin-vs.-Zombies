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
        GetControl<Button>("Setting").onClick.AddListener(OpenSetting);
        GetControl<Button>("Exit").onClick.AddListener(Exit);
    }
    protected override void BeforeHide()
    {
        GetControl<Button>("Start").onClick.RemoveAllListeners();
        GetControl<Button>("Setting").onClick.RemoveAllListeners();
    }
    void StartGame()
    {
        Hide();
        UIManager.Instance.ShowPanel<LevelsPanel>("LevelsPanel");

    }
    void OpenSetting()
    {

        Hide();
        UIManager.Instance.ShowPanel<SettingPanel>("SettingPanel");
    }
    void Exit()
    {
        Application.Quit(0);
    }
}
