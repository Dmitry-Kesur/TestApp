using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.Factories;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Models.UI;
using Infrastructure.Providers;
using Infrastructure.Services.PlayerProgressUpdaters;
using Infrastructure.StateMachine;

namespace Infrastructure.Services
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
        private readonly ILevelFactory _levelFactory;

        private List<LevelPreviewModel> _levelPreviews;

        private LevelModel _currentLevelModel;

        public LevelService(LevelsStaticDataProvider levelsStaticDataProvider,
            LevelProgressUpdater levelProgressUpdater, IReceiveRewardsService receiveRewardsService,
            AnalyticsService analyticsService, StateMachineService stateMachine, ILevelFactory levelFactory, IExceptionLoggerService exceptionLoggerService)
        {
            _levelsStaticDataProvider = levelsStaticDataProvider;
            _levelProgressUpdater = levelProgressUpdater;
            _receiveRewardsService = receiveRewardsService;
            _analyticsService = analyticsService;
            _stateMachine = stateMachine;
            _levelFactory = levelFactory;
            _exceptionLoggerService = exceptionLoggerService;

            _levelsStaticDataProvider.OnLevelsDataLoaded = OnLevelsDataLoaded;
        }

        public void Start()
        {
            if (_currentLevelModel is {Started: true})
            {
                Resume();
                return;
            }

            _currentLevelModel?.OnStart();
        }

        public void Select(int level)
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

        public void Stop() =>
            _currentLevelModel.Stop();

        public LevelModel GetCurrentLevel() =>
            _currentLevelModel;

        public bool ReachedMaxLevel =>
            _currentLevelModel.Level == _levelsStaticDataProvider.MaxLevel;

        public List<LevelPreviewModel> GetPreviewsModels() =>
            _levelPreviews;

        private void CreatePreviewModels()
        {
            _levelPreviews = new List<LevelPreviewModel>();

            var levelsData = _levelsStaticDataProvider.GetLevelsData();
            foreach (var levelData in levelsData)
            {
                var previewModel = _levelFactory.CreatePreviewModel(levelData);
                _levelPreviews.Add(previewModel);
            }
        }

        private void Resume() =>
            _currentLevelModel.OnResume();

        private LevelPreviewModel GetLevelPreview(int level) =>
            _levelPreviews.Find(model => model.Level == level);

        private void UpdateNextLevel()
        {
            if (ReachedMaxLevel) return;
            
            var nextLevel = GetNextLevel();
            CreateLevelModel(nextLevel);
            SetActiveStateLevelPreview(nextLevel);
            
            Select(nextLevel);
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
            
            SetCompleteStateLevelPreview(currentLevel);
            
            UpdateNextLevel();

            _stateMachine.TransitionTo(StateType.WinLevelState);
        }

        private void SetCompleteStateLevelPreview(int level)
        {
            var previewModel = GetLevelPreview(level);
            previewModel.IsActive = false;
            previewModel.IsComplete = true;
        }

        private void SetActiveStateLevelPreview(int level)
        {
            var previewModel = GetLevelPreview(level);
            previewModel.IsActive = true;
            previewModel.IsComplete = false;
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
            var levelModel = _levelFactory.CreateModel(levelData);
            SubscribeListeners(levelModel);
            _levelModels.Add(levelModel);
        }

        private void OnLevelsDataLoaded()
        {
            CreateLevelModel(_levelProgressUpdater.GetActiveLevel());
            CreatePreviewModels();
        }
    }
}