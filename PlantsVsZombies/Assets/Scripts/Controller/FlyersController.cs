using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����������
/// </summary>
public class FlyersController
{
    private static GameObject FlyersFatherObject = new GameObject("Flyers");
    /// <summary>
    /// ��ӷ�����
    /// </summary>
    /// <param name="data">����������</param>
    /// <param name="worldPos">��������ֵ���������</param>
    /// <returns>���������</returns>
    public Flyer AddFlyer(IFlyerData data ,Vector3 worldPos)
    {
        //TODO:�ø������������������괦���һ�����������
        //�����������������٣���˲���Ҫ���桢���ҹ���
        GameObject flyer = GameObject.Instantiate(data.OriginalReference, FlyersFatherObject.transform);
        data.GameObject = flyer;
        flyer.transform.position = worldPos;
        return flyer.GetComponent<Flyer>();
    }
}
