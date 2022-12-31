using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// �����������
/// </summary>
public class FlyersController
{
    public readonly static GameObject FlyersFatherObject = new GameObject("Flyers");
    /// <summary>
    /// �����
    /// ��ԭʼԤ������ΪKey�����ɵ�ʵ���б�ΪValue
    /// ���������������Ԥ��������
    /// </summary>
    private ObjectBuffer flyerPool = new ObjectBuffer(FlyersFatherObject.transform);
    /// <summary>
    /// ���������弯��
    /// </summary>
    private List<Flyer> flyers = new List<Flyer>();

    /// <summary>
    /// ��ӷ�����
    /// ������������ݺͷ�����ű����˫������IFlyerData.Gameobject��Flyer.Data��
    /// </summary>
    /// <param name="data">����������</param>
    /// <param name="worldPos">��������ֵ���������</param>
    /// <param name="callBack">�������ڼ���ǰ���õĺ���</param>
    /// <returns>���������Ľű�</returns>
    public void AddFlyer(IFlyerData data, Vector3 worldPos, UnityAction<Flyer> callback) => AddFlyer<Flyer>(data,worldPos, callback);
    /// <summary>
    /// ��ӷ��������
    /// </summary>
    /// <typeparam name="T">ת���Ľű�����</typeparam>
    /// <param name="data">����������</param>
    /// <param name="worldPos">��������ֵ���������</param>
    /// <param name="callBack">�������ڼ���ǰ���õĺ���</param>
    /// <returns>���������ű�</returns>
    public T AddFlyer<T>(IFlyerData data, Vector3 worldPos, UnityAction<T> callBack = null) where T : Flyer 
    {
        GameObject obj = flyerPool.Get(data.OriginalReference);
        obj.transform.position = worldPos;
        //����˫������
        T flyer = obj.GetComponent<T>();//��ȡ���ϵķ�����ű�
        flyer.Data = data;
        data.GameObject = obj;

        callBack?.Invoke(flyer);//���ú���
        flyers.Add(flyer);

        return flyer;

    }
    /// <summary>
    /// ��������ӳ������Ƴ�
    /// ȡ�����������ݺͷ�����ű�֮���˫������
    /// </summary>
    /// <param name="flyer">Ҫ�Ƴ��ķ�����</param>
    public void RemoveFlyer(Flyer flyer)
    {
        flyers.Remove(flyer);
        flyerPool.Put(flyer.Data.OriginalReference, flyer.gameObject);

        //ȡ����������
        flyer.Data.GameObject = null;
        flyer.Data = null;

    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="action"></param>
    public void Foreach(System.Action<Flyer> action)
    {
        flyers.ForEach(action);
    }
    public void Clear()
    {
        foreach(Flyer flyer in flyers)
        {
            flyer.Data.GameObject = null;
            GameObject.Destroy(flyer.gameObject);
        }
        flyerPool.Clear();
    }
}
