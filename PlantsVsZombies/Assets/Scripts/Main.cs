using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour
{
    void Start()
    {
        Run();
    }
    public void Run()
    {
        AudioManager.Instance.PlayRandomBackgroundMusic();
        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && GameController.Instance.IsGameStarted)
        {

            Vector2 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameController.Instance.FlyersController.AddFlyer<PeaBulletBehaviour>(FlyerPrefabSerializer.Instance.GetFlyerData("IceBullet"), world, (bullet)=>
            {
                bullet.AvailableArea = new FrontLine();//�ӵ��ķ�ΧΪǰһ��  
                bullet.ElementType = Elements.Ice;//�ı��ӵ���Ԫ���˺�
                bullet.Damage = 10;//�ӵ����˺��빥���ߵĹ�������ͬ
                bullet.CanAddElement = true;//�ӵ�һֱ���Ը���Ԫ��
            });
        }
    }
}
