using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class MenuWindowModel : BaseWindowModel
    {
        private readonly GameHandler _gameHandler;
        private readonly GameScoreModel _gameScoreModel;
        
        public MenuWindowModel(GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
            _gameScoreModel = _gameHandler.ScoreModel;
        }

        public int gameScore => _gameScoreModel.GameScore;

        public float gameScoreMultiplier => _gameScoreModel.scoreMultiplier;
        
        public override BaseWindow GetWindowInstance()
        {
            var instance = GameObject.Instantiate(Resources.Load<MenuWindow>("Prefabs/UI/Windows/MenuWindow"));
            instance.Init(this);
            return instance;
        }

        public void PlayButtonClick()
        {
            _gameHandler.stateMachine.SetState(StateName.GameSession);
        }

        public void OnSettingsButtonClick()
        {
            _gameHandler.stateMachine.SetState(StateName.SettingsState);
        }
    }
}