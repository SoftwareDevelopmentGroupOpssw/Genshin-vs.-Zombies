using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 面板基类
/// </summary>
public class BasePanel : MonoBehaviour
{

    protected Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    protected virtual void Awake()
    {
        FindChildrenControls<Button>();
        FindChildrenControls<Image>();
        FindChildrenControls<Slider>();
        FindChildrenControls<Text>();
        FindChildrenControls<Dropdown>();
        FindChildrenControls<InputField>();
    }

    /// <summary>
    /// 从子物体上找到组件群
    /// </summary>
    /// <typeparam name="T">需要的控件类型</typeparam>
    protected void FindChildrenControls<T>() where T : UIBehaviour
    {
        T[] controls = GetComponentsInChildren<T>();
        foreach (T control in controls)
        {
            string objName = control.gameObject.name;

            if (controlDic.ContainsKey(objName))
                controlDic[objName].Add(control);
            else
            {
                controlDic.Add(objName, new List<UIBehaviour>());
                controlDic[objName].Add(control);
            }
        }
    }

    /// <summary>
    /// 从某个物体上找到某个组件
    /// </summary>
    /// <typeparam name="T">需要的组件类型</typeparam>
    /// <param name="controlName">子物体的名称</param>
    /// <returns>所需的组件</returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (controlDic.ContainsKey(controlName))
        {
            foreach (UIBehaviour behaviour in controlDic[controlName])
            {
                if (behaviour is T)
                    return behaviour as T;
            }
        }

        return null;
    }

    public void AddButtonListener(string ButtonName, UnityAction call)
    {
        GetControl<Button>(ButtonName).onClick.AddListener(call);
    }

    public void AddSliderListener(string sliderName, UnityAction<float> call)
    {
        GetControl<Slider>(sliderName).onValueChanged.AddListener(call);
    }

    #region Show
    protected virtual void BeforeShow()
    {

    }
    protected virtual void AfterShow()
    {

    }

    public void Show()
    {
        BeforeShow();
        gameObject.SetActive(true);
        AfterShow();
    }
    #endregion

    #region Hide
    protected virtual void BeforeHide()
    {

    }
    protected virtual void AfterHide()
    {

    }
    public void Hide()
    {
        BeforeHide();
        gameObject.SetActive(false);
        AfterHide();
    }
    #endregion
}
