using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔物逻辑更新器
/// </summary>
public class MonstersUpdater
{
    private MonstersController controller;
    public MonstersUpdater(MonstersController controller) => this.controller = controller;

    /// <summary>
    /// 帧更新
    /// </summary>
    public void Update()
    {
        controller.Foreach((monster) =>
        {
            if (monster.Data.CanAction)
                monster.Data.Action();
        });
    }
}