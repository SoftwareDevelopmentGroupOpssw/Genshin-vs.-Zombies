using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public partial class GameController
{
    /// <summary>
    /// �ܸ���ģ�飺�������еĸ���ģ��
    /// ��������Ϸ����������ģ��
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
