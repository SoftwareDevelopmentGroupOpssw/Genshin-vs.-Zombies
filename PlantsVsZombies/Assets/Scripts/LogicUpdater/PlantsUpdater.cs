using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ֲ���߼�������
/// </summary>
public class PlantsUpdater
{
    private PlantsController controller;
    public PlantsUpdater(PlantsController controller) => this.controller = controller;
   
    /// <summary>
    /// ֡����
    /// </summary>
    public void Update()
    {
        controller.Foreach((plant) =>
        {
            
        });
    }
}
