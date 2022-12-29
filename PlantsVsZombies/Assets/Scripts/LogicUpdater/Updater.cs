using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ¸üÐÂÄ£¿é
/// </summary>
public class Updater
{
    private RefreshModule refresh;
    public Updater(MonstersController monstersController,Queue<IMonsterData> monsterList)
    {
        refresh = new RefreshModule(monstersController,monsterList);
    }
    // Update is called once per frame
    public void Update()
    {
        refresh.Update();
    }
}
