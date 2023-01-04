using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour
{
    /// <summary>
    /// 背景音乐协程，音乐结束时会播放另外一个随机背景音乐
    /// </summary>
    /// <returns></returns>
    IEnumerator BackgroundMusicCoroutine()
    {
        AudioSource source = AudioManager.Instance.MusicSource;
        AudioClip clip = AudioManager.Instance.PlayRandomBackgroundMusic();
        while(source.isPlaying || !source.isPlaying && source.time < clip.length) //正在播放或者被暂停
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
                bullet.AvailableArea = new FrontLine();//子弹的范围为前一行  
                bullet.ElementType = Elements.Ice;//改变子弹的元素伤害
                bullet.Damage = 10;//子弹的伤害与攻击者的攻击力相同
                bullet.CanAddElement = true;//子弹一直可以附着元素
            });
        }
    }
}
