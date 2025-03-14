using Infrastructure.Enums;
using Infrastructure.Factories;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [Inject] private IStatesFactory _statesFactory;
        [Inject] private StateMachineService _stateMachine;
        [Inject] private IProgressService _progressService;

        private void Start()
        {
            PrepareGame();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveProgress();
        }

        private void OnApplicationQuit()
        {
            SaveProgress();
        }

        private void PrepareGame()
        {
            var states = _statesFactory.CreateStates();
            _stateMachine.SetStates(states);
            _stateMachine.TransitionTo(StateType.InitializeThirdPartyServicesState);
        }

        private void SaveProgress()
        {
            _progressService.SavePlayerProgress();
        }
    }
}