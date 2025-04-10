using System.Collections.Generic;
using Infrastructure.Controllers.Levels;
using Infrastructure.Enums;
using Infrastructure.Factories.Level;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI.Items;
using Infrastructure.Providers.Level;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Log;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;
using Infrastructure.Services.Reward;
using Infrastructure.StateMachine;

namespace Infrastructure.Services.Level
{
    public class LevelService : ILevelsService
    {
        private readonly List<LevelModel> _levelModels = new();

        private readonly LevelsStaticDataProvider _levelsStaticDataProvider;
        private readonly LevelProgressUpdater _levelProgressUpdater;
        private readonly IReceiveRewardsService _receiveRewardsService;
        private readonly AnalyticsService _analyticsService;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly StateMachineService _stateMachine;
        private readonly ILevelModelsFactory _levelModelsFactory;
        private readonly LevelPreviewsController _previewsController;

        private LevelModel _currentLevelModel;

        public LevelService(LevelsStaticDataProvider levelsStaticDataProvider,
            LevelProgressUpdater levelProgressUpdater, IReceiveRewardsService receiveRewardsService,
            AnalyticsService analyticsService, StateMachineService stateMachine, ILevelModelsFactory levelModelsFactory, IExceptionLoggerService exceptionLoggerService)
        {
            _levelsStaticDataProvider = levelsStaticDataProvider;
            _levelProgressUpdater = levelProgressUpdater;
            _receiveRewardsService = receiveRewardsService;
            _analyticsService = analyticsService;
            _stateMachine = stateMachine;
            _levelModelsFactory = levelModelsFactory;
            _exceptionLoggerService = exceptionLoggerService;

            _previewsController = new LevelPreviewsController(_levelModelsFactory);
        }

        public void Start()
        {
            _currentLevelModel?.Start();
        }

        public void SetCurrentLevel(int level)
        {
            _currentLevelModel = GetLevel(level);
            if (_currentLevelModel == null)
            {
                _exceptionLoggerService.LogError($"[level-service] Failed to get {level} level model at select");
                return;
            }
            
            _levelProgressUpdater.ChangeActiveLevel(level);
        }

        public void Pause() =>
            _currentLevelModel.OnPause();

        public void Resume() =>
            _currentLevelModel.OnResume();

        public void Initialize()
        {
            _previewsController.CreatePreviews(_levelsStaticDataProvider.GetLevelsData());
            CreateLevelModel(_levelProgressUpdater.GetActiveLevel());
        }

        public void Stop() =>
            _currentLevelModel.Stop();

        public LevelModel GetCurrentLevel() =>
            _currentLevelModel;

        public bool ReachedMaxLevel =>
            _currentLevelModel.Level == _levelsStaticDataProvider.MaxLevel;

        public bool LevelStarted => 
            _currentLevelModel is { Started: true };

        public List<LevelPreviewModel> GetPreviewsModels() =>
            _previewsController.GetPreviewsModels();

        private void UpdateNextLevel()
        {
            if (ReachedMaxLevel) return;
            
            var nextLevel = GetNextLevel();
            CreateLevelModel(nextLevel);
            _previewsController.MarkPreviewAsActive(nextLevel);
            
            SetCurrentLevel(nextLevel);
        }

        private void SubscribeListeners(LevelModel levelModel)
        {
            levelModel.OnLoseAction = OnLose;
            levelModel.OnWinAction = OnWin;
        }

        private void OnWin()
        {
            _receiveRewardsService.ReceiveRewards(_currentLevelModel.GetRewards());

            var currentLevel = _currentLevelModel.Level;

            _levelProgressUpdater.SetCompleteLevel(currentLevel);
            _analyticsService.LogWinLevel(currentLevel);
            
            _previewsController.MarkPreviewAsComplete(currentLevel);
            
            UpdateNextLevel();

            _stateMachine.TransitionTo(StateType.WinLevelState);
        }

        private int GetNextLevel() =>
            _currentLevelModel.Level + 1;

        private void OnLose()
        {
            _analyticsService.LogLoseLevel(_currentLevelModel.Level);
            _stateMachine.TransitionTo(StateType.LoseLevelState);
        }

        private LevelModel GetLevel(int level) =>
            _levelModels.Find(model => model.Level == level);

        private void CreateLevelModel(int level)
        {
            var levelData = _levelsStaticDataProvider.GetDataByLevel(level);
            var levelModel = _levelModelsFactory.CreateModel(levelData);
            SubscribeListeners(levelModel);
            _levelModels.Add(levelModel);
        }
    }
}