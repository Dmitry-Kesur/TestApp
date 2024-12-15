using Infrastructure.Enums;
using Infrastructure.Factories;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private SceneContext sceneContext;

        private void Start()
        {
            PrepareGame();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                SaveProgress();
        }

        private void PrepareGame()
        {
            var statesFactory = sceneContext.Container.Resolve<IStatesFactory>();
            var states = statesFactory.CreateStates();
            var stateMachine = sceneContext.Container.Resolve<StateMachineService>();
            stateMachine.SetStates(states);
            stateMachine.TransitionTo(StateType.AuthenticationState);
        }

        private void SaveProgress()
        {
            var playerProgressService = sceneContext.Container.Resolve<IPlayerProgressService>();
            playerProgressService.SavePlayerProgress();
        }
    }
}