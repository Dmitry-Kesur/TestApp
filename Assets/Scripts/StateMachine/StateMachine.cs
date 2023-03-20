using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class StateMachine
{
    private readonly Dictionary<StateName, BaseState> _states;
    private BaseState _currentState;

    public StateMachine()
    {
        _states = new Dictionary<StateName, BaseState>();
    }

    public void AddState(StateName stateName, BaseState state)
    {
        _states.Add(stateName, state);
    }
    
    public void SetState(StateName stateName)
    {
        _currentState = _states[stateName];
        _currentState.OnStateEnter();
        Debug.Log($"[State Machine] Set state to [ {_currentState.GetType().Name} ]");
    }
}