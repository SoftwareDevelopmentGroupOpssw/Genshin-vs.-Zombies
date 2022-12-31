using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameController
{
    /// <summary>
    /// 刷新模块,负责魔物的刷新 隶属于游戏控制器
    /// </summary>
    class RefreshModule
    {
        private bool isGenerateCompleted = false;
        /// <summary>
        /// 生成新的魔物
        /// </summary>
        public void GenerateNewMonster(IMonsterData data)
        {
            ILevelData level = GameController.Instance.LevelData;

            System.Random random = new System.Random();
            int row = random.Next(1, level.Row);//随机选一行

            Vector3 worldPos = GameController.Instance.GridToWorld(new Vector2Int(level.Col, row), GridPosition.Right);
            GameController.Instance.MonstersController.AddMonster(data, worldPos);
        }

        private bool isResultShowed = false;
        /// <summary>
        /// 帧更新时调用，尝试生成魔物
        /// </summary>
        public void Update()
        {
            if (generateCoroutine == null)
                generateCoroutine = MonoManager.Instance.StartCoroutine(GenerateCoroutine());
            
            if(GameController.Instance.MonstersController.MonsterCount == 0 && isGenerateCompleted && !isResultShowed)
            {
                GameController.Instance.ShowResult(true);//赢得游戏
                isResultShowed = true;
            }
        }
        private Coroutine generateCoroutine;

        /// <summary>
        /// 生成魔物协程
        /// </summary>
        /// <returns></returns>
        IEnumerator GenerateCoroutine()
        {
            IEnumerator enumerator = GameController.Instance.LevelData.GenerateEnumerator();
            enumerator.MoveNext();//移动到第一个元素
            do
            {
                //是一个整数，直接生成怪物
                if (enumerator.Current is IMonsterData)
                {
                    GenerateNewMonster(enumerator.Current as IMonsterData);//帧更新结束时尝试生成魔物
                    yield return new WaitForSecondsRealtime(0.5f);//魔物生成不会连续，总有半秒的延迟
                }
                else //在生成策略中，仍然可以用unity协程中的yield instructions来延迟时间
                    yield return enumerator.Current;

                while (GameController.Instance.IsPaused)
                {
                    yield return 1;
                }

            } while (enumerator.MoveNext() && GameController.Instance.IsGameStarted);
            isGenerateCompleted = true;
        }
    }

}
