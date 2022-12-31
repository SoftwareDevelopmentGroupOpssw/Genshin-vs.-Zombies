using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefeatPanel : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void BeforeShow()
    {
        GetControl<Button>("RestartBtn").onClick.AddListener(Restart);
        GetControl<Button>("TitleBtn").onClick.AddListener(ToTitle);
    }
    protected override void BeforeHide()
    {
        GetControl<Button>("RestartBtn").onClick.RemoveListener(Restart);
        GetControl<Button>("TitleBtn").onClick.RemoveListener(ToTitle);
    }
    void ToTitle()
    {
        GameController.Instance.EndGame();
        UIManager.Instance.HidePanel("DefeatPanel");
        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
    }
    void Restart()
    {
        GameController.Instance.EndGame();
        UIManager.Instance.HidePanel("DefeatPanel");
        GameController.Instance.StartGame();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
