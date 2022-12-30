using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 超绽放产生的爆炸控制脚本
/// </summary>
public class HyperExplosion : MonoBehaviour
{
    /// <summary>
    /// 追踪弹对象
    /// </summary>
    public GameObject hyperSeed;

    /// <summary>
    /// 追踪弹在开始追踪前被弹射出去的半径
    /// </summary>
    public const float ExplodeRadius = 0.6f;
    
    /// <summary>
    /// 爆炸，并产生一定数量的追踪弹
    /// </summary>
    /// <param name="count">追踪弹数量</param>
    public void Explode(int count)
    {
        for(float i = 0; i < count; i++)
        {
            
            float nowRotation = i / count * 360;
            GameObject seed = Instantiate(hyperSeed, FlyersController.FlyersFatherObject.transform);
            seed.transform.position = transform.position;//将种子出现位置和爆炸位置重合
            seed.transform.eulerAngles = new Vector3(0, 0, nowRotation);//改变种子发射出去的方向
        }
        Destroy(gameObject);
    }
}
