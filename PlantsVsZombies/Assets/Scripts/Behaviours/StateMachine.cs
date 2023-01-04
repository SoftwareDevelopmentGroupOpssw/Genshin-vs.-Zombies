using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ״̬��
/// </summary>
/// <typeparam name="TState">״̬����</typeparam>
/// <typeparam name="TStateKey">״̬�ļ�������Ѱ��״̬��</typeparam>
/// <typeparam name="TActionKey">�¼��ļ�������Ѱ���¼���������</typeparam>
public class StateMachine<TState,TStateKey,TActionKey>
{
    private Dictionary<TStateKey,Node> nodes = new Dictionary<TStateKey, Node>();
    private Dictionary<TActionKey, List<Action>> actions = new Dictionary<TActionKey, List<Action>>();
    
    private TStateKey current;
    /// <summary>
    /// ����������״̬
    /// </summary>
    public TState Current => nodes[current].state;
    /// <summary>
    /// ����������״̬�ļ�
    /// </summary>
    public TStateKey CurrentKey => current;
    
    /// <summary>
    /// Ĭ��״̬�ļ�
    /// </summary>
    private TStateKey defaultStateKey;

    /// <summary>
    /// Ϊ���״̬������һ��Ĭ��״̬
    /// Reset��ʱ��Ὣ״̬�����ó�Ĭ��״̬
    /// </summary>
    /// <param name="defaultStateKey">Ĭ��״̬�ļ�</param>
    /// <param name="defaultState">Ĭ��״̬</param>
    public StateMachine(TStateKey defaultStateKey, TState defaultState)
    {
        this.defaultStateKey = defaultStateKey;
        current = defaultStateKey;
        AddState(defaultStateKey,defaultState);
    }
    /// <summary>
    /// Ϊ���״̬������һ��Ĭ��״̬
    /// Reset��ʱ��Ὣ״̬�����ó�Ĭ��״̬
    /// </summary>
    /// <param name="defaultStateKey">Ĭ��״̬�ļ�</param>
    /// <param name="defaultState">Ĭ��״̬</param>
    /// <param name="onStateEnter">Ĭ��״̬���뺯��</param>
    /// <param name="onStateExit">Ĭ��״̬�˳�����</param>
    public StateMachine(TStateKey defaultStateKey, TState defaultState, System.Action onStateEnter, System.Action onStateExit)
    {
        this.defaultStateKey = defaultStateKey;
        current = defaultStateKey;
        AddState(defaultStateKey,defaultState,onStateEnter, onStateExit);
    }
    /// <summary>
    /// ���һ��״̬���
    /// </summary>
    /// <param name="stateKey">״̬�ļ�</param>
    /// <param name="state">״̬</param>
    /// <exception cref="StateMachineException.StateAlreadyExistsException">��ǰ״̬�Ѵ���</exception>
    public void AddState(TStateKey stateKey, TState state)
    {
        if (!nodes.ContainsKey(stateKey))
            nodes.Add(stateKey, new Node() { state = state});
        else
            throw new StateMachineException.StateAlreadyExistsException("The state " + stateKey + " already exists !");
    }

    /// <summary>
    /// ���һ��״̬��㲢���ý�����˳�����
    /// </summary>
    /// <param name="stateKey">״̬�ļ�</param>
    /// <param name="state">״̬</param>
    /// <param name="onStateEnter">�������״̬ʱ����</param>
    /// <param name="onStateExit">�뿪���״̬ʱ����</param>
    /// <exception cref="StateMachineException.StateAlreadyExistsException">��ǰ״̬�Ѵ���</exception>
    public void AddState(TStateKey stateKey, TState state, System.Action onStateEnter, System.Action onStateExit)
    {
        if (!nodes.ContainsKey(stateKey))
            nodes.Add(stateKey, new Node()
            {
                state = state,
                onEnter = onStateEnter,
                onExit = onStateExit
            }); 
        else
            throw new StateMachineException.StateAlreadyExistsException("The state " + state + " already exists !");
    }

    /// <summary>
    /// ���һ���¼�����״̬
    /// ��״̬��������ᴴ��
    /// </summary>
    /// <param name="state">��Ҫ����¼���״̬</param>
    /// <param name="actionKey">�¼�id</param>
    /// <param name="action">�¼�</param>
    /// <exception cref="StateMachineException.StateNotFoundException">�¼�����ʼ״̬�����״̬������</exception>
    /// <exception cref="StateMachineException.ActionMismatchException">�¼�����ʼ״̬�ʹ�״̬��ƥ��</exception>
    /// <exception cref="StateMachineException.ActionAlreadyExistsException">�¼��ļ��Ѵ���</exception>
    public void AddAction(TActionKey actionKey, params Action[] addingActions)
    {
        foreach(var action in addingActions)
        {
            if (!nodes.ContainsKey(action.StartStateKey))//�¼�����ʼ״̬������
                throw new StateMachineException.StateNotFoundException("The start state " + action.StartStateKey + " cannot be found !");
            if (!nodes.ContainsKey(action.EndStateKey)) //�¼��Ľ���״̬������
                throw new StateMachineException.StateNotFoundException("The end state " + action.EndStateKey + " cannot be found !");
            if (actions.ContainsKey(actionKey))
            {
                foreach (Action item in actions[actionKey])
                {
                    if (item.StartStateKey.Equals(action.StartStateKey))
                    {
                        throw new StateMachineException.ActionAlreadyExistsException
                            ("An action with same start state was already added to the machine. That is not allowed. " +
                            $"Action to be added:{action}; Existing action:{item}");
                    }
                }
                actions[actionKey].Add(action);
            }
            else
            {
                actions.Add(actionKey, new List<Action>() { action });
            }
        }

    }
    /// <summary>
    /// ����һ���¼���ʹ״̬�ƶ���������״̬
    /// </summary>
    /// <param name="actionKey">�������¼��ļ�</param>
    /// <exception cref="StateMachineException.InvalidTriggerException">�������¼�����ʼ״̬�͵�ǰ״̬��һ��</exception>
    public void TriggerAction(TActionKey actionKey)
    {
        if (actions.ContainsKey(actionKey))
        {
            Action action = default(Action);
            foreach(Action item in actions[actionKey])
            {
                if(item.StartStateKey.Equals(current))
                {
                    action = item;
                    break;
                }
            }
            if (action.Equals(default(Action)))
            {
                Debug.LogWarning(new StateMachineException.InvalidTriggerException($"No suitable action with key {actionKey} can be triggered."));
            }
            else
            {
                nodes[current].onExit?.Invoke();
                current = action.EndStateKey;
                nodes[current].onEnter?.Invoke();
            }
        }
    }
    /// <summary>
    /// ��״̬����ΪĬ��״̬�����õ�ǰ״̬���˳�������Ĭ��״̬�Ľ��뺯��
    /// </summary>
    public void Reset()
    {
        nodes[current].onExit?.Invoke();
        current = defaultStateKey;
        nodes[defaultStateKey].onEnter?.Invoke();
    }
    public TState this[TStateKey stateKey]
    {
        get
        {
            if (nodes.ContainsKey(stateKey))
                return nodes[stateKey].state;
            else
                return default(TState);
        }
    }
    public Action[] this[TActionKey actionKey]
    {
        get
        {
            if (actions.ContainsKey(actionKey))
                return actions[actionKey].ToArray();
            else
                return new Action[0];
        }
    }
    public struct Action
    {
        public TStateKey StartStateKey { get; set; }
        public TStateKey EndStateKey { get; set; }
        public Action(TStateKey start, TStateKey end)
        {
            this.StartStateKey = start;
            this.EndStateKey = end;
        }
        public override string ToString()
        {
            return $"({StartStateKey},{EndStateKey})";
        }
    }

    /// <summary>
    /// һ��״̬���
    /// </summary>
    struct Node
    {
        /// <summary>
        /// ���ڵ�״̬
        /// </summary>
        public TState state;
        /// <summary>
        /// �����״̬ʱ����
        /// </summary>
        public System.Action onEnter;
        /// <summary>
        /// �뿪��״̬ʱ����
        /// </summary>
        public System.Action onExit;
    }


}
public class StateMachineException : System.Exception
{
    public StateMachineException(string msg) : base(msg) { }
    /// <summary>
    /// ��ǰ״̬�Ѵ���
    /// </summary>
    public class StateAlreadyExistsException : StateMachineException
    {
        public StateAlreadyExistsException(string msg) : base(msg) { }
    }
    /// <summary>
    /// ��ǰ״̬δ�ҵ�
    /// </summary>
    public class StateNotFoundException : StateMachineException
    {
        public StateNotFoundException(string msg) : base(msg) { }
    }
    /// <summary>
    /// Action��ȡֵ�뵱ǰ��ӵ�State��ƥ��
    /// </summary>
    public class ActionMismatchException : StateMachineException
    {
        public ActionMismatchException(string msg) : base(msg) { }
    }
    /// <summary>
    /// ��ǰ�¼��Ѵ���
    /// </summary>
    public class ActionAlreadyExistsException : StateMachineException
    {
        public ActionAlreadyExistsException(string msg) : base(msg) { }
    }
    /// <summary>
    /// �Ƿ����������������¼�����ʼ״̬�͵�ǰ״̬��һ�µ���
    /// </summary>
    public class InvalidTriggerException : StateMachineException
    {
        public InvalidTriggerException(string msg) : base(msg) { }
    }
}