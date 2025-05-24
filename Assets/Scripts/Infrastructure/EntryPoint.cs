using Infrastructure.Enums;
using Infrastructure.Factories.State;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [Inject] private IStatesFactory _statesFactory;
        [Inject] private StateMachineService _stateMachine;

        private void Start()
        {
            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            var states = _statesFactory.CreateStates();
            _stateMachine.SetStates(states);
            _stateMachine.TransitionTo(StateType.InitializeThirdPartyServicesState);
        }
    }
}