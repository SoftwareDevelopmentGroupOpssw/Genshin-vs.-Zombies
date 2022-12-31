using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public partial class GameController
{
    /// <summary>
    /// 总更新模块：控制所有的更新模块
    /// 隶属于游戏控制器的子模块
    /// </summary>
    class Updater
    {
        private RefreshModule refresh;
        public Updater()
        {
            refresh = new RefreshModule();
        }
        // Update is called once per frame
        public void Update()
        {
            refresh.Update();
        }
    }
}
