using System.Collections.Generic;
using Infrastructure.Controllers.Levels;
using Infrastructure.Data.Level;
using Infrastructure.Enums;
using Infrastructure.Factories.Level;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI.Items;
using Infrastructure.Providers.Level;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Log;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Reward;
using Infrastructure.StateMachine;
using Infrastructure.Views.GameEntities;
using UnityEngine;

namespace Infrastructure.Services.Level
{
    public class LevelService : ILevelsService, IBootstrapTarget
    {
        private readonly LevelsStaticDataProvider _levelsStaticDataProvider;
        private readonly SaveLoadProgressService _saveLoadProgressService;
        private readonly IReceiveRewardsService _receiveRewardsService;
        private readonly AnalyticsService _analyticsService;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly StateMachineService _stateMachine;
        private readonly LevelFactory _levelFactory;
        private readonly LevelPreviewsController _previewsController;
        private readonly LevelViewsFactory _levelViewsFactory;

        private LevelStaticData _selectedLevelData;
        private LevelModel _currentLevelModel;
        private LevelResult _levelResult;

        private LevelView _levelView;

        public LevelService(LevelsStaticDataProvider levelsStaticDataProvider,
            SaveLoadProgressService saveLoadProgressService, IReceiveRewardsService receiveRewardsService,
            AnalyticsService analyticsService, StateMachineService stateMachine, LevelFactory levelFactory,
            IExceptionLoggerService exceptionLoggerService, LevelPreviewsController levelPreviewsController, LevelViewsFactory levelViewsFactory)
        {
            _levelsStaticDataProvider = levelsStaticDataProvider;
            _saveLoadProgressService = saveLoadProgressService;
            _receiveRewardsService = receiveRewardsService;
            _analyticsService = analyticsService;
            _stateMachine = stateMachine;
            _levelFactory = levelFactory;
            _exceptionLoggerService = exceptionLoggerService;

            _previewsController = levelPreviewsController;
            _levelViewsFactory = levelViewsFactory;
        }

        public void StartLevel()
        {
            EnsureLevelSession();
            if (_currentLevelModel == null)
            {
                _exceptionLoggerService.LogError("Level session is null");
                return;
            }
            
            _currentLevelModel.SetData(_selectedLevelData);
            _currentLevelModel.Start();
        }

        public void SelectLevel(int level)
        {
            var levelData = _levelsStaticDataProvider.GetDataByLevel(level);
            if (levelData == null)
            {
                _exceptionLoggerService.LogError($"No static data for level {level}");
                return;
            }
            
            _selectedLevelData = levelData;

            _saveLoadProgressService.Write(progress => progress.ActiveLevel = level);
        }

        public void Pause() =>
            _currentLevelModel.Pause();

        public void Revive()
        {
            _currentLevelModel.Revive();
        }

        public void Resume() => _currentLevelModel.Resume();

        public void Restart()
        {
            _currentLevelModel.Stop();
            StartLevel();
        }

        public int InitializationOrder => 4;

        public void Initialize()
        {
            _previewsController.CreatePreviews(_levelsStaticDataProvider.GetLevelsData());
        }

        public void Stop() =>
            _currentLevelModel.Stop();

        public LevelModel GetCurrentLevel() =>
            _currentLevelModel;

        public bool ReachedMaxLevel =>
            _currentLevelModel != null && _currentLevelModel.Level == _levelsStaticDataProvider.MaxLevel;

        public LevelResult LevelResult => _levelResult;

        public List<LevelPreviewModel> GetPreviewsModels() =>
            _previewsController.GetPreviewsModels();

        private void UpdateNextLevel()
        {
            if (ReachedMaxLevel) return;

            var nextLevel =  _selectedLevelData.Level + 1;
            SelectLevel(nextLevel);

            _previewsController.MarkPreviewAsActive(nextLevel);
        }

        private void SubscribeListeners(LevelModel levelModel)
        {
            levelModel.OnLoseAction += OnLose;
            levelModel.OnWinAction += OnWin;
            levelModel.OnStartedAction += OnStarted;
            levelModel.OnStoppedAction += OnStopped;
        }

        private void OnStarted()
        {
            _levelView = _levelViewsFactory.CreateLevelView(_currentLevelModel);
        }

        private void OnStopped()
        {
            if (_levelView == null)
                return;

            _levelView.BeforeDestroy();
            Object.Destroy(_levelView.gameObject);
            _levelView = null;
        }

        private void OnWin()
        {
            _receiveRewardsService.ReceiveRewards(_currentLevelModel.GetRewards());

            var currentLevel = _selectedLevelData.Level;

            _saveLoadProgressService.Write(progress => progress.AddWinLevel(currentLevel));
            _analyticsService.LogWinLevel(currentLevel);

            _previewsController.MarkPreviewAsComplete(currentLevel);

            _levelResult = new LevelResult(_currentLevelModel.TotalLevelScore);
            
            _stateMachine.TransitionTo(StateType.WinLevelState);
            
            UpdateNextLevel();
        }

        private void OnLose()
        {
            _analyticsService.LogLoseLevel(_selectedLevelData.Level);
            _stateMachine.TransitionTo(StateType.LoseLevelState);
        }

        private void EnsureLevelSession()
        {
            if (_currentLevelModel != null)
                return;

            var levelSession = _levelFactory.CreateLevelSession();
            SubscribeListeners(levelSession);
            _currentLevelModel = levelSession;
        }
    }
}