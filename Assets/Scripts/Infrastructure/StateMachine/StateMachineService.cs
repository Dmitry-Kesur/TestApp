using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.StateMachine.States;

namespace Infrastructure.StateMachine
{
    public class StateMachineService
    {
        private Dictionary<StateType, State> _states = new();
        private State _currentState;

        public void SetStates(Dictionary<StateType, State> states)
        {
            _states = states;
        }

        public void TransitionTo(StateType stateType)
        {
            _currentState?.Exit();
            _currentState = _states[stateType];
            _currentState.Enter();
        }
    }
}