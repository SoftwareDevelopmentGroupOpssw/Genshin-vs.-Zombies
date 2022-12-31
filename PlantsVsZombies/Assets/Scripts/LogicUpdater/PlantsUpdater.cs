using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 植物逻辑更新器
/// </summary>
public class PlantsUpdater
{
    private PlantsController controller;
    public PlantsUpdater(PlantsController controller) => this.controller = controller;
   
    /// <summary>
    /// 帧更新
    /// </summary>
    public void Update()
    {
        controller.Foreach((plant) =>
        {
            
        });
    }
}
