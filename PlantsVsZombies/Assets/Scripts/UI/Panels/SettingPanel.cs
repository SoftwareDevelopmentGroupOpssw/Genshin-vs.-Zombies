using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    protected override void BeforeShow()
    {
        if (GameController.Instance.IsGameStarted)
        {
            InGame();
            GetControl<Button>("RestartBtn").onClick.AddListener(Restart);
            GetControl<Button>("ResumeBtn").onClick.AddListener(Resume);
        }
        else
        {
            InTitle();
        }

        GetControl<Button>("TitleBtn").onClick.AddListener(Title);
        Slider musicBar = GetControl<Slider>("MusicBar");
        Slider soundBar = GetControl<Slider>("SoundBar");
        musicBar.value = AudioManager.Instance.MusicVolume;
        musicBar.onValueChanged.AddListener(ChangeMusicVolume);
        soundBar.value = AudioManager.Instance.EffectVolume;
        soundBar.onValueChanged.AddListener(ChangeSoundVolume);
    }
    protected override void BeforeHide()
    {
        GetControl<Button>("RestartBtn").onClick.RemoveAllListeners();
        GetControl<Button>("ResumeBtn").onClick.RemoveAllListeners();
        GetControl<Button>("TitleBtn").onClick.RemoveAllListeners();

        GetControl<Slider>("MusicBar").onValueChanged.RemoveAllListeners();
        GetControl<Slider>("SoundBar").onValueChanged.RemoveAllListeners();
    }
    void ChangeMusicVolume(float value) => AudioManager.Instance.MusicVolume = value;
    void ChangeSoundVolume(float value) => AudioManager.Instance.EffectVolume = value;

    void Resume()
    {
        Hide();
        GameController.Instance.Resume();
    }
    void Title()
    {
        Hide();
        if(GameController.Instance.IsGameStarted)
            GameController.Instance.EndGame();
        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
    }
    void Restart()
    {
        GameController.Instance.EndGame();
        Hide();
        GameController.Instance.StartGame();
    }
    /// <summary>
    /// 正在游戏中时对设置面板的调整
    /// </summary>
    void InGame()
    {
        GetControl<Button>("ResumeBtn").gameObject.SetActive(true);
        GetControl<Button>("RestartBtn").gameObject.SetActive(true);
    }
    /// <summary>
    /// 在标题页面中对设置面板的调整
    /// </summary>
    void InTitle()
    {
        GetControl<Button>("ResumeBtn").gameObject.SetActive(false);
        GetControl<Button>("RestartBtn").gameObject.SetActive(false);
    }
}
