using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// ������
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
    /// �����������ҵ����Ⱥ
    /// </summary>
    /// <typeparam name="T">��Ҫ�Ŀؼ�����</typeparam>
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
    /// ��ĳ���������ҵ�ĳ�����
    /// </summary>
    /// <typeparam name="T">��Ҫ���������</typeparam>
    /// <param name="controlName">�����������</param>
    /// <returns>��������</returns>
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
