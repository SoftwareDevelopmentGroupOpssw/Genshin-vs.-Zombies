using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����������
/// </summary>
public class FlyersController
{
    private readonly static GameObject FlyersFatherObject = new GameObject("Flyers");
    /// <summary>
    /// ��ӷ�����
    /// </summary>
    /// <param name="data">����������</param>
    /// <param name="worldPos">��������ֵ���������</param>
    /// <returns>���������Ľű�</returns>
    public Flyer AddFlyer(IFlyerData data ,Vector3 worldPos)
    {
        //TODO:�ø������������������괦���һ�����������
        //�����������������٣���˲���Ҫ���桢���ҹ���
        GameObject flyer = GameObject.Instantiate(data.OriginalReference, FlyersFatherObject.transform);
        data.GameObject = flyer;
        flyer.transform.position = worldPos;
        return flyer.GetComponent<Flyer>();
    }
    /// <summary>
    /// ��ӷ��������
    /// </summary>
    /// <typeparam name="T">ת���Ľű�����</typeparam>
    /// <param name="data">����������</param>
    /// <param name="worldPos">��������ֵ���������</param>
    /// <returns>���������ű�</returns>
    public T AddFlyer<T>(IFlyerData data, Vector3 worldPos) where T : Flyer => AddFlyer(data, worldPos) as T;
}
