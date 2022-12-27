using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ħ���߼�������
/// </summary>
public class MonstersUpdater
{
    private MonstersController controller;
    public MonstersUpdater(MonstersController controller) => this.controller = controller;

    /// <summary>
    /// ֡����
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