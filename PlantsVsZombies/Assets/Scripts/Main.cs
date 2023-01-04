using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour
{
    /// <summary>
    /// ��������Э�̣����ֽ���ʱ�Ქ������һ�������������
    /// </summary>
    /// <returns></returns>
    IEnumerator BackgroundMusicCoroutine()
    {
        AudioSource source = AudioManager.Instance.MusicSource;
        AudioClip clip = AudioManager.Instance.PlayRandomBackgroundMusic();
        while(source.isPlaying || !source.isPlaying && source.time < clip.length) //���ڲ��Ż��߱���ͣ
        {
            yield return 1;
            if (AudioManager.Instance.MusicSource.time == clip.length)
                break;
        }
        StartCoroutine(BackgroundMusicCoroutine());
    }
    void Start()
    {
        Run();
    }
    public void Run()
    {
        StartCoroutine(BackgroundMusicCoroutine());
        UIManager.Instance.ShowPanel<TitlePanel>("TitlePanel");
    }
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift) && GameController.Instance.IsGameStarted)
        {

            Vector2 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameController.Instance.FlyersController.AddFlyer<PeaBulletBehaviour>(FlyerPrefabSerializer.Instance.GetFlyerData("IceBullet"), world, (bullet) =>
            {
                bullet.AvailableArea = new FrontLine();//�ӵ��ķ�ΧΪǰһ��  
                bullet.ElementType = Elements.Ice;//�ı��ӵ���Ԫ���˺�
                bullet.Damage = 10;//�ӵ����˺��빥���ߵĹ�������ͬ
                bullet.CanAddElement = true;//�ӵ�һֱ���Ը���Ԫ��
            });
        }
    }
}
