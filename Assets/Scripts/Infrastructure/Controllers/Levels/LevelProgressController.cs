using System;
using Infrastructure.Models.GameEntities.Level;
using Infrastructure.Services.Booster;
using Infrastructure.Services.Hud;
using Infrastructure.Services.Progress;

namespace Infrastructure.Controllers.Levels
{
    public class LevelProgressController
    {
        private readonly IHudService _hudService;
        private readonly IBoostersService _boostersService;
        private readonly SaveLoadProgressService _saveLoadProgressService;

        public Action OnReachScoreToWin;
        public Action OnReachedMaximumFailItems;

        private int _totalFailItems;
        private int _totalLevelScore;

        private LevelSession _levelSession;

        public LevelProgressController(IHudService hudService, IBoostersService boostersService,
            SaveLoadProgressService saveLoadProgressService)
        {
            _hudService = hudService;
            _boostersService = boostersService;
            _saveLoadProgressService = saveLoadProgressService;
        }

        public int TotalLevelScore =>
            _totalLevelScore;

        public int TotalFailItems =>
            _totalFailItems;

        public void UpdateProgressByFailItem()
        {
            _totalFailItems++;

            _hudService.UpdateHud();

            if (_totalFailItems == _levelSession.MaximumFailItems)
            {
                UpdateBestScore();
                OnReachedMaximumFailItems?.Invoke();
            }
        }

        public void Clear()
        {
            _totalLevelScore = 0;
            ClearFailedProgress();
        }

        public void SetModel(LevelSession levelSession) =>
            _levelSession = levelSession;

        public void Refresh()
        {
            _hudService.UpdateHud();
        }

        public void UpdateProgressByCatchItem(int scorePoints)
        {
            _totalLevelScore += GetUpdatedScore(scorePoints);

            _hudService.UpdateHud();

            if (TotalLevelScore >= _levelSession.ScorePointsToWin)
            {
                UpdateBestScore();
                OnReachScoreToWin?.Invoke();
            }
        }

        public void ClearFailedProgress() =>
            _totalFailItems = 0;

        private void UpdateBestScore()
        {
            var currentBestScore = _saveLoadProgressService.Read(progress => progress.BestScore);
            if (currentBestScore < _totalLevelScore)
                _saveLoadProgressService.Write(progress => progress.BestScore = _totalLevelScore);
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