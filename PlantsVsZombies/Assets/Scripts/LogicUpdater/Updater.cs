using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ¸üÐÂÄ£¿é
/// </summary>
public class Updater
{
    private PlantsController plantsController;
    private MonstersController monstersController;
    private PlantsUpdater plantsUpdater;
    private MonstersUpdater monstersUpdater;
    private RefreshModule refresh;
    public Updater(PlantsController plantsController,MonstersController monstersController)
    {
        this.plantsController = plantsController;
        this.monstersController = monstersController;
        plantsUpdater = new PlantsUpdater(plantsController);
        monstersUpdater = new MonstersUpdater(monstersController);
        refresh = new RefreshModule();
    }
    // Update is called once per frame
    public void Update()
    {
        plantsUpdater.Update();
        monstersUpdater.Update();
        refresh.Update();
    }
}
