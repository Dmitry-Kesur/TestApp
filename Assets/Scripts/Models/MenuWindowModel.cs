using System;
using SimpleInjector;
using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class MenuWindowModel : BaseWindowModel
    {
        public Action OnPlayButtonClickAction;
        public Action OnSettingsButtonClickAction;
        private MenuWindow _instance;
        private readonly GameScoreModel _gameScoreModel;

        public MenuWindowModel(Container dependencyInjectionContainer)
        {
            var gameDataController = dependencyInjectionContainer.GetInstance<GameDataService>();
            _gameScoreModel = gameDataController.GetGameScoreModel();
        }

        public int gameScore => _gameScoreModel.gameScore;

        public float gameScoreMultiplier => _gameScoreModel.scoreMultiplier;

        public override BaseWindow GetWindowInstance()
        {
            if (windowInstance == null)
            {
                windowInstance = GameObject.Instantiate(Resources.Load<MenuWindow>("Prefabs/UI/Windows/MenuWindow"));
                windowInstance.Init(this);   
            }
            
            return windowInstance;
        }

        public void PlayButtonClick()
        {
            OnPlayButtonClickAction?.Invoke();
        }

        public void OnSettingsButtonClick()
        {
            OnSettingsButtonClickAction?.Invoke();
        }
    }
}