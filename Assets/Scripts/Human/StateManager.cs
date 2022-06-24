 using UnityEngine;
using System.Collections.Generic;
using System;

public class StateManager : MonoBehaviour
{
    public List<State> states;
    public State currentState;
    public State defaultState;
    public HumanBase human;

    public virtual void Init()
    {
        foreach(State state in states)
        {
            state.SetManager(this);
            state.Init();
        }

        human = transform.GetComponent<HumanBase>();
        Debug.Log("#State" + human.name + "状态机初始化完成");
    }

    protected virtual void FixedUpdate()
    {
        if (currentState == null)
        {
            currentState = defaultState;
        }
        currentState.FixedUpdate();
    }

    protected virtual void Update()
    {
        if (currentState == null)
        {
            currentState = defaultState;
        }

        currentState.Update();
    }

    public void ChangeState<T>() where T : State
    {
        //print("切换至：" + typeof(T).Name);
        State _temp = null;
        foreach (State state in states)
        {
            if (state is T)
            {
                _temp = state;
                break;
            }
        }
        if (_temp == null)
        {
            Debug.LogWarning(this.GetType().FullName + "使用了一个未注册的状态" + typeof(T).Name + "。切换状态失败。");
        }
        else
        {
            State last = currentState;
            if (currentState != null)
                currentState.OnExitState();
            currentState = _temp;
            currentState.OnEnterState(last);
        }
    }

    public State GetCurrentState()
    {
        return currentState;
    }

    public T GetState<T>() where T : State
    {
        foreach (State state in states)
        {
            if (state is T)
            {
                return (T)state;
            }
        }
        Debug.LogWarning(this.GetType().FullName + "尝试获取了一个未注册的状态" + nameof(T) + "。获取状态失败。");
        return null;
    }

    public void OnDestroy()
    {
        currentState.OnExitState();
        foreach (var state in states)
        {
            Destroy(state);
        }
    }

    public static void CloneStates(StateManager _originalManager)
    {
        List<State> _newStates = new List<State>();
        foreach (State _state in _originalManager.states)
        {
            State _newState = ScriptableObject.Instantiate(_state);
            _newStates.Add(_newState);
            if (_state == _originalManager.defaultState)
            {
                _originalManager.defaultState = _newState;
            }
        }
        _originalManager.states = _newStates;
    }
}
