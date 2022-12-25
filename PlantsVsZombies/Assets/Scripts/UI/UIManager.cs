using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// 游戏中所有的UI面板的管理者
/// </summary>
public class UIManager:Singleton<UIManager>
{
    const string UI_PATH = "Prefabs/UI/";

    /// <summary>
    /// UI面板显示的层级
    /// </summary>
    public enum UILayer
    {
        Top,
        Mid,
        Bot,
        System
    }

    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform top;  //顶层
    private Transform mid;  //中层
    private Transform bot;  //底层
    private Transform system;   //系统层

    [Obsolete("不应使用new方法创建UIManager",true)]
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
    /// 获取指定UI层级的Transform属性
    /// </summary>
    /// <param name="layer">层级枚举</param>
    /// <returns>指定层级的Transform</returns>
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
    /// 用类型来显示面板
    /// </summary>
    /// <typeparam name="T">面板的类型</typeparam>
    /// <param name="panelName">面板名</param>
    /// <param name="layer">要加载的层级(默认中层)</param>
    /// <param name="callback">回调函数，当面板创建完成后调用</param>
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

            //得到预设体身上的面板脚本
            T panel = instantiated.GetComponent<T>();
            // 处理面板创建完成后的逻辑
            callback?.Invoke(panel);

            panel.Show();

            //把面板存起来
            panelDic.Add(panelName, panel);
        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName">panel名称</param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Hide();
        }
    }
    /// <summary>
    /// 关闭所有面板
    /// </summary>
    public void HideAllPanel()
    {
        foreach (var item in panelDic)
        {
            item.Value.Hide();
        }
    }
    /// <summary>
    /// 得到某一个已经显示的面板 方便外部使用
    /// </summary>
    public T GetPanel<T>(string name) where T : BasePanel
    {
        if (panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }
    #endregion

    /// <summary>
    /// 给某个控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="call">事件的响应函数</param>
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
