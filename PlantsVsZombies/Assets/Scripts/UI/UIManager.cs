using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// ��Ϸ�����е�UI���Ĺ�����
/// </summary>
public class UIManager:Singleton<UIManager>
{
    const string UI_PATH = "Prefabs/UI/";

    /// <summary>
    /// UI�����ʾ�Ĳ㼶
    /// </summary>
    public enum UILayer
    {
        Top,
        Mid,
        Bot,
        System
    }

    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform top;  //����
    private Transform mid;  //�в�
    private Transform bot;  //�ײ�
    private Transform system;   //ϵͳ��

    [Obsolete("��Ӧʹ��new��������UIManager",true)]
    public UIManager()
    {
        GameObject obj = null;
        obj = ResourceManager.Instance.Load<GameObject>(UI_PATH + "Canvas");
        GameObject.DontDestroyOnLoad(obj);

        top = obj.transform.Find("Top");
        mid = obj.transform.Find("Mid");
        bot = obj.transform.Find("Bot");
        system = obj.transform.Find("System");

        obj = ResourceManager.Instance.Load<GameObject>(UI_PATH + "EventSystem");
        GameObject.DontDestroyOnLoad(obj);
    }

    /// <summary>
    /// ��ȡָ��UI�㼶��Transform����
    /// </summary>
    /// <param name="layer">�㼶ö��</param>
    /// <returns>ָ���㼶��Transform</returns>
    public Transform GetUILayer(UILayer layer)
    {
        switch (layer)
        {
            case UILayer.Top:
                return top;

            case UILayer.Mid:
                return mid;

            case UILayer.Bot:
                return bot;

            case UILayer.System:
                return system;
        }
        return null;
    }

    #region PanelOperate
    /// <summary>
    /// ����������ʾ���
    /// </summary>
    /// <typeparam name="T">��������</typeparam>
    /// <param name="panelName">�����</param>
    /// <param name="layer">Ҫ���صĲ㼶(Ĭ���в�)</param>
    /// <param name="callback">�ص�����������崴����ɺ����</param>
    public void ShowPanel<T>(string panelName, UILayer layer = UILayer.Mid, UnityAction<T> callback = null) where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Show();

            if (callback != null)
                callback(panelDic[panelName] as T);

            return;
        }

        ResourceManager.Instance.LoadAsync<GameObject>(UI_PATH + "Panels/" + panelName, (obj) =>
        {
            GameObject instantiated = GameObject.Instantiate(obj, GetUILayer(layer));

            //�õ�Ԥ�������ϵ����ű�
            T panel = instantiated.GetComponent<T>();
            // ������崴����ɺ���߼�
            callback?.Invoke(panel);

            panel.Show();

            //����������
            panelDic.Add(panelName, panel);
        });
    }

    /// <summary>
    /// �������
    /// </summary>
    /// <param name="panelName">panel����</param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Hide();
        }
    }
    /// <summary>
    /// �ر��������
    /// </summary>
    public void HideAllPanel()
    {
        foreach (var item in panelDic)
        {
            item.Value.Hide();
        }
    }
    /// <summary>
    /// �õ�ĳһ���Ѿ���ʾ����� �����ⲿʹ��
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }
    #endregion

    /// <summary>
    /// ��ĳ���ؼ�����Զ����¼�����
    /// </summary>
    /// <param name="control">�ؼ�����</param>
    /// <param name="type">�¼�����</param>
    /// <param name="call">�¼�����Ӧ����</param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> call)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(call);

        trigger.triggers.Add(entry);
    }

}
