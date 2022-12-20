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
    /// ���Ӹ����¼�
    /// </summary>
    /// <param name="action"></param>
    public void AddUpdateListener(UnityAction action)
    {
        updateEvent += action;
    }

    /// <summary>
    /// �Ƴ������¼�
    /// </summary>
    /// <param name="action"></param>
    public void RemoveUpdateListener(UnityAction action)
    {
        updateEvent -= action;
    }

}
