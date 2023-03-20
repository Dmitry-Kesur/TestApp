using SimpleInjector;
using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class MenuWindowModel : BaseWindowModel
    {
        private readonly GameScoreModel _gameScoreModel;
        private readonly StateMachine _stateMachine;

        public MenuWindowModel(Container dependencyInjectionContainer)
        {
            var gameDataController = dependencyInjectionContainer.GetInstance<GameDataController>();
            _gameScoreModel = gameDataController.GetGameScoreModel();

            _stateMachine = dependencyInjectionContainer.GetInstance<StateMachine>();
        }

        public int gameScore => _gameScoreModel.gameScore;

        public float gameScoreMultiplier => _gameScoreModel.scoreMultiplier;

        public override BaseWindow GetWindowInstance()
        {
            var instance = GameObject.Instantiate(Resources.Load<MenuWindow>("Prefabs/UI/Windows/MenuWindow"));
            instance.Init(this);
            return instance;
        }

        public void PlayButtonClick()
        {
            _stateMachine.SetState(StateName.GameSession);
        }

        public void OnSettingsButtonClick()
        {
            _stateMachine.SetState(StateName.SettingsState);
        }
    }
}