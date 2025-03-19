using System;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Services;
using Infrastructure.Services.Booster;
using Infrastructure.Services.Hud;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;

namespace Infrastructure.Controllers.Levels
{
    public class ProgressController
    {
        private readonly IHudService _hudService;
        private readonly IBoostersService _boostersService;
        private readonly LevelProgressUpdater _levelProgressUpdater;

        public Action OnReachScoreToWin;
        public Action OnReachedMaximumFailItems;
        public Action OnUpdateProgress;

        private int _totalFailItems;
        private int _totalLevelScore;
        private int _totalCatchItems;
        
        private ILevelModel _levelModel;

        public ProgressController(IHudService hudService, IBoostersService boostersService, LevelProgressUpdater levelProgressUpdater)
        {
            _hudService = hudService;
            _boostersService = boostersService;
            _levelProgressUpdater = levelProgressUpdater;
        }
        
        public int TotalLevelScore =>
            _totalLevelScore;

        public int TotalFailItems =>
            _totalFailItems;

        public int TotalCatchItems =>
            _totalCatchItems;

        public void UpdateProgressByFailItem()
        {
            _totalFailItems++;
            
            OnUpdateProgress?.Invoke();
            _hudService.UpdateHud();

            if (_totalFailItems == _levelModel.FailItemsMaximum)
            {
                UpdateBestScore();
                OnReachedMaximumFailItems?.Invoke();
            }
        }

        public void Clear()
        {
            _totalLevelScore = 0;
            _totalFailItems = 0;
            _totalCatchItems = 0;
        }

        public void SetModel(ILevelModel levelModel) =>
            _levelModel = levelModel;

        public void UpdateProgressByCatchItem(int scorePoints)
        {
            _totalCatchItems++;
            _totalLevelScore += GetUpdatedScore(scorePoints);
            
            OnUpdateProgress?.Invoke();
            _hudService.UpdateHud();

            if (TotalLevelScore >= _levelModel.ScorePointsToWin)
            {
                UpdateBestScore();
                OnReachScoreToWin?.Invoke();
            }
        }

        private void UpdateBestScore()
        {
            var currentBestScore = _levelProgressUpdater.GetBestScore();
            if (currentBestScore < _totalLevelScore)
                _levelProgressUpdater.UpdateBestScore(_totalLevelScore);
        }

        private int GetUpdatedScore(int scorePoints)
        {
            var boostValue = _boostersService.BoostValue;

            if (boostValue == 0)
                return scorePoints;
            
            return scorePoints * boostValue;
        }
    }
}