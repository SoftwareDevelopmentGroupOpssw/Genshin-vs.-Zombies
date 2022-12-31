using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameController
{
    /// <summary>
    /// ˢ��ģ��,����ħ���ˢ�� ��������Ϸ������
    /// </summary>
    class RefreshModule
    {
        private bool isGenerateCompleted = false;
        /// <summary>
        /// �����µ�ħ��
        /// </summary>
        public void GenerateNewMonster(IMonsterData data)
        {
            ILevelData level = GameController.Instance.LevelData;

            System.Random random = new System.Random();
            int row = random.Next(1, level.Row);//���ѡһ��

            Vector3 worldPos = GameController.Instance.GridToWorld(new Vector2Int(level.Col, row), GridPosition.Right);
            GameController.Instance.MonstersController.AddMonster(data, worldPos);
        }

        private bool isResultShowed = false;
        /// <summary>
        /// ֡����ʱ���ã���������ħ��
        /// </summary>
        public void Update()
        {
            if (generateCoroutine == null)
                generateCoroutine = MonoManager.Instance.StartCoroutine(GenerateCoroutine());
            
            if(GameController.Instance.MonstersController.MonsterCount == 0 && isGenerateCompleted && !isResultShowed)
            {
                GameController.Instance.ShowResult(true);//Ӯ����Ϸ
                isResultShowed = true;
            }
        }
        private Coroutine generateCoroutine;

        /// <summary>
        /// ����ħ��Э��
        /// </summary>
        /// <returns></returns>
        IEnumerator GenerateCoroutine()
        {
            IEnumerator enumerator = GameController.Instance.LevelData.GenerateEnumerator();
            enumerator.MoveNext();//�ƶ�����һ��Ԫ��
            do
            {
                //��һ��������ֱ�����ɹ���
                if (enumerator.Current is IMonsterData)
                {
                    GenerateNewMonster(enumerator.Current as IMonsterData);//֡���½���ʱ��������ħ��
                    yield return new WaitForSecondsRealtime(0.5f);//ħ�����ɲ������������а�����ӳ�
                }
                else //�����ɲ����У���Ȼ������unityЭ���е�yield instructions���ӳ�ʱ��
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
