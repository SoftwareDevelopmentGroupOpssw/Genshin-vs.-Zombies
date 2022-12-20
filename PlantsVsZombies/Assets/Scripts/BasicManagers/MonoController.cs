using UnityEngine;
using UnityEngine.Events;

class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;

    private void Update()
    {
        if (updateEvent != null)
            updateEvent();
    }

    /// <summary>
    /// 增加更新事件
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }

    /// <summary>
    /// 移除更新事件
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }

}
