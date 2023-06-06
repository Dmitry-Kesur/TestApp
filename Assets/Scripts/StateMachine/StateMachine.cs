using System.Collections.Generic;
using DefaultNamespace;
using SimpleInjector;

public class StateMachine
{
    private Dictionary<StateType, BaseState> _states;
    private List<StateType> _stateTypes;
    private BaseState _currentState;
    private Container _container;

    public void Init(Container container)
    {
        _container = container;
        _states = new Dictionary<StateType, BaseState>();
        _stateTypes = new List<StateType> {StateType.InitState, StateType.MenuState, StateType.SettingsState, StateType.GameSession};

        InitStates();
    }

    private void InitStates()
    {
        foreach (var stateType in _stateTypes)
        {
            var state = StateFactory.CreateState(stateType, _container);
            state.ChangeStateAction = SetState;
            AddState(stateType, state);
        }
        
        SetState(StateType.InitState);
    }

    private void AddState(StateType stateType, BaseState state)
    {
        _states.Add(stateType, state);
    }

    private void SetState(StateType stateType)
    {
        var previousState = _currentState;
        _currentState = _states[stateType];
        _currentState.OnStateChanged(previousState);
    }
}