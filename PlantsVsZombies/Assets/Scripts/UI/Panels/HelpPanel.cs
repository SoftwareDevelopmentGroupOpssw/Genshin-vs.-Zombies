using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ËµÃ÷Ãæ°å
/// </summary>
public class HelpPanel : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    protected override void BeforeShow()
    {
        GetControl<Button>("BackBtn").onClick.AddListener(ToTile);
    }
    protected override void BeforeHide()
    {
        GetControl<Button>("BackBtn").onClick.RemoveListener(ToTile);
    }

    void ToTile()
    {
        AudioManager.Instance.PlayEffectAudio("buttonclick");
        Hide();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
