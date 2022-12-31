using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ų����ı�ը���ƽű�
/// </summary>
public class HyperExplosion : MonoBehaviour
{
    /// <summary>
    /// ׷�ٵ�����
    /// </summary>
    public GameObject hyperSeed;

    /// <summary>
    /// ׷�ٵ��ڿ�ʼ׷��ǰ�������ȥ�İ뾶
    /// </summary>
    public const float ExplodeRadius = 0.6f;
    
    /// <summary>
    /// ��ը��������һ��������׷�ٵ�
    /// </summary>
    /// <param name="count">׷�ٵ�����</param>
    public void Explode(int count)
    {
        for(float i = 0; i < count; i++)
        {
            
            float nowRotation = i / count * 360;
            GameObject seed = Instantiate(hyperSeed, FlyersController.FlyersFatherObject.transform);
            seed.transform.position = transform.position;//�����ӳ���λ�úͱ�ըλ���غ�
            seed.transform.eulerAngles = new Vector3(0, 0, nowRotation);//�ı����ӷ����ȥ�ķ���
        }
        Destroy(gameObject);
    }
}
