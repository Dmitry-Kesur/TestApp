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
        private readonly StateMachineService _stateMachine;
        private readonly ILevelFactory _levelFactory;

        private List<LevelPreviewModel> _levelPreviews;

        private LevelModel _currentLevelModel;

        public LevelService(LevelsStaticDataProvider levelsStaticDataProvider,
            LevelProgressUpdater levelProgressUpdater, IReceiveRewardsService receiveRewardsService,
            AnalyticsService analyticsService, StateMachineService stateMachine, ILevelFactory levelFactory)
        {
            _levelsStaticDataProvider = levelsStaticDataProvider;
            _levelProgressUpdater = levelProgressUpdater;
            _receiveRewardsService = receiveRewardsService;
            _analyticsService = analyticsService;
            _stateMachine = stateMachine;
            _levelFactory = levelFactory;
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
            var levelModel = GetOrCreateLevel(level);
            _currentLevelModel = levelModel;
            _levelProgressUpdater.ChangeActiveLevel(_currentLevelModel.Level);
        }

        public void Pause() =>
            _currentLevelModel.OnPause();

        public void Stop() =>
            _currentLevelModel.Stop();

        public LevelModel GetCurrentLevel() =>
            _currentLevelModel;

        public bool ReachedMaxLevel =>
            _currentLevelModel.Level == _levelsStaticDataProvider.GetLevelsData().Count;

        public List<LevelPreviewModel> GetOrCreatePreviewsModels() =>
            _levelPreviews ??= CreateLevelPreviews();

        private List<LevelPreviewModel> CreateLevelPreviews()
        {
            var previewModels = new List<LevelPreviewModel>();

            var levelsData = _levelsStaticDataProvider.GetLevelsData();
            var activeLevel = _levelProgressUpdater.GetActiveLevel();
            var completeLevels = _levelProgressUpdater.GetCompleteLevelIds();

            foreach (var levelData in levelsData)
            {
                var previewModel = new LevelPreviewModel(levelData);

                if (levelData.Level == activeLevel)
                    previewModel.IsActive = true;

                else if (completeLevels.Contains(levelData.Level))
                    previewModel.IsComplete = true;

                previewModels.Add(previewModel);
            }

            return previewModels;
        }

        private void Resume() =>
            _currentLevelModel.OnResume();

        private LevelPreviewModel GetLevelPreview(int level) =>
            _levelPreviews.Find(model => model.Level == level);

        private void SelectNextLevel()
        {
            if (ReachedMaxLevel) return;

            var nextLevel = GetNextLevel();
            Select(nextLevel);

            SetActiveLevelPreview(_currentLevelModel.Level);
        }

        private void SubscribeListeners(LevelModel levelModel)
        {
            levelModel.OnLoseAction = OnLose;
            levelModel.OnWinAction = OnWin;
        }

        private void OnWin()
        {
            _receiveRewardsService.ReceiveRewards(_currentLevelModel.GetRewards());

            var level = _currentLevelModel.Level;

            _levelProgressUpdater.SetCompleteLevel(level);
            _analyticsService.LogWinLevel(level);

            _stateMachine.TransitionTo(StateType.WinLevelState);

            SetCompleteLevelPreview(level);

            SelectNextLevel();
        }

        private void SetCompleteLevelPreview(int level)
        {
            var previewModel = GetLevelPreview(level);
            previewModel.IsActive = false;
            previewModel.IsComplete = true;
        }

        private void SetActiveLevelPreview(int level)
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

        private LevelModel GetOrCreateLevel(int level)
        {
            LevelModel levelModel = _levelModels.Find(model => model.Level == level);
            if (levelModel != null)
                return levelModel;

            return CreateLevelModel(level);
        }

        private LevelModel CreateLevelModel(int level)
        {
            var levelData = _levelsStaticDataProvider.GetDataByLevel(level);
            var levelModel = _levelFactory.CreateModel(levelData);
            SubscribeListeners(levelModel);
            _levelModels.Add(levelModel);
            return levelModel;
        }
    }
}