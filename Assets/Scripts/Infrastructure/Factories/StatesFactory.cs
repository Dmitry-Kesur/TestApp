using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.StateMachine.States;
using Zenject;

namespace Infrastructure.Factories
{
    public class StatesFactory : IStatesFactory
    {
        private readonly DiContainer _diContainer;

        public StatesFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public Dictionary<StateType, State> CreateStates()
        {
            Dictionary<StateType, State> states = new()
            {
                {StateType.LoadingState, _diContainer.Instantiate<LoadingState>()},
                {StateType.MenuState, _diContainer.Instantiate<MenuState>()},
                {StateType.SettingsState, _diContainer.Instantiate<SettingsState>()},
                {StateType.GameLoopState, _diContainer.Instantiate<GameLoopState>()},
                {StateType.LoseLevelState, _diContainer.Instantiate<LoseLevelState>()},
                {StateType.WinLevelState, _diContainer.Instantiate<WinLevelState>()},
                {StateType.PauseGameLoopState, _diContainer.Instantiate<PauseGameLoopState>()},
                {StateType.SelectLevelState, _diContainer.Instantiate<SelectLevelState>()},
                {StateType.ShopState, _diContainer.Instantiate<ShopState>()},
                {StateType.BoostersState, _diContainer.Instantiate<BoostersState>()},
                {StateType.AuthenticationState, _diContainer.Instantiate<AuthenticationState>()}
            };

            return states;
        }
    }
}