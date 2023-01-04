using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态机
/// </summary>
/// <typeparam name="TState">状态类型</typeparam>
/// <typeparam name="TStateKey">状态的键（用来寻找状态）</typeparam>
/// <typeparam name="TActionKey">事件的键（用来寻找事件并触发）</typeparam>
public class StateMachine<TState,TStateKey,TActionKey>
{
    private Dictionary<TStateKey,Node> nodes = new Dictionary<TStateKey, Node>();
    private Dictionary<TActionKey, List<Action>> actions = new Dictionary<TActionKey, List<Action>>();
    
    private TStateKey current;
    /// <summary>
    /// 现在所处的状态
    /// </summary>
    public TState Current => nodes[current].state;
    /// <summary>
    /// 现在所处的状态的键
    /// </summary>
    public TStateKey CurrentKey => current;
    
    /// <summary>
    /// 默认状态的键
    /// </summary>
    private TStateKey defaultStateKey;

    /// <summary>
    /// 为这个状态机设置一个默认状态
    /// Reset的时候会将状态机设置成默认状态
    /// </summary>
    /// <param name="defaultStateKey">默认状态的键</param>
    /// <param name="defaultState">默认状态</param>
    public StateMachine(TStateKey defaultStateKey, TState defaultState)
    {
        this.defaultStateKey = defaultStateKey;
        current = defaultStateKey;
        AddState(defaultStateKey,defaultState);
    }
    /// <summary>
    /// 为这个状态机设置一个默认状态
    /// Reset的时候会将状态机设置成默认状态
    /// </summary>
    /// <param name="defaultStateKey">默认状态的键</param>
    /// <param name="defaultState">默认状态</param>
    /// <param name="onStateEnter">默认状态进入函数</param>
    /// <param name="onStateExit">默认状态退出函数</param>
    public StateMachine(TStateKey defaultStateKey, TState defaultState, System.Action onStateEnter, System.Action onStateExit)
    {
        this.defaultStateKey = defaultStateKey;
        current = defaultStateKey;
        AddState(defaultStateKey,defaultState,onStateEnter, onStateExit);
    }
    /// <summary>
    /// 添加一个状态结点
    /// </summary>
    /// <param name="stateKey">状态的键</param>
    /// <param name="state">状态</param>
    /// <exception cref="StateMachineException.StateAlreadyExistsException">当前状态已存在</exception>
    public void AddState(TStateKey stateKey, TState state)
    {
        if (!nodes.ContainsKey(stateKey))
            nodes.Add(stateKey, new Node() { state = state});
        else
            throw new StateMachineException.StateAlreadyExistsException("The state " + stateKey + " already exists !");
    }

    /// <summary>
    /// 添加一个状态结点并设置进入和退出动作
    /// </summary>
    /// <param name="stateKey">状态的键</param>
    /// <param name="state">状态</param>
    /// <param name="onStateEnter">进入这个状态时调用</param>
    /// <param name="onStateExit">离开这个状态时调用</param>
    /// <exception cref="StateMachineException.StateAlreadyExistsException">当前状态已存在</exception>
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
    /// 添加一个事件到此状态
    /// 若状态不存在则会创建
    /// </summary>
    /// <param name="state">需要添加事件的状态</param>
    /// <param name="actionKey">事件id</param>
    /// <param name="action">事件</param>
    /// <exception cref="StateMachineException.StateNotFoundException">事件的起始状态或结束状态不存在</exception>
    /// <exception cref="StateMachineException.ActionMismatchException">事件的起始状态和此状态不匹配</exception>
    /// <exception cref="StateMachineException.ActionAlreadyExistsException">事件的键已存在</exception>
    public void AddAction(TActionKey actionKey, params Action[] addingActions)
    {
        foreach(var action in addingActions)
        {
            if (!nodes.ContainsKey(action.StartStateKey))//事件的起始状态不存在
                throw new StateMachineException.StateNotFoundException("The start state " + action.StartStateKey + " cannot be found !");
            if (!nodes.ContainsKey(action.EndStateKey)) //事件的结束状态不存在
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
    /// 触发一个事件，使状态移动到触发的状态
    /// </summary>
    /// <param name="actionKey">触发的事件的键</param>
    /// <exception cref="StateMachineException.InvalidTriggerException">触发的事件的起始状态和当前状态不一致</exception>
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
    /// 将状态设置为默认状态，调用当前状态的退出函数和默认状态的进入函数
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
    /// 一个状态结点
    /// </summary>
    struct Node
    {
        /// <summary>
        /// 现在的状态
        /// </summary>
        public TState state;
        /// <summary>
        /// 进入此状态时调用
        /// </summary>
        public System.Action onEnter;
        /// <summary>
        /// 离开此状态时调用
        /// </summary>
        public System.Action onExit;
    }


}
public class StateMachineException : System.Exception
{
    public StateMachineException(string msg) : base(msg) { }
    /// <summary>
    /// 当前状态已存在
    /// </summary>
    public class StateAlreadyExistsException : StateMachineException
    {
        public StateAlreadyExistsException(string msg) : base(msg) { }
    }
    /// <summary>
    /// 当前状态未找到
    /// </summary>
    public class StateNotFoundException : StateMachineException
    {
        public StateNotFoundException(string msg) : base(msg) { }
    }
    /// <summary>
    /// Action的取值与当前添加的State不匹配
    /// </summary>
    public class ActionMismatchException : StateMachineException
    {
        public ActionMismatchException(string msg) : base(msg) { }
    }
    /// <summary>
    /// 当前事件已存在
    /// </summary>
    public class ActionAlreadyExistsException : StateMachineException
    {
        public ActionAlreadyExistsException(string msg) : base(msg) { }
    }
    /// <summary>
    /// 非法触发：当触发的事件的起始状态和当前状态不一致调用
    /// </summary>
    public class InvalidTriggerException : StateMachineException
    {
        public InvalidTriggerException(string msg) : base(msg) { }
    }
}