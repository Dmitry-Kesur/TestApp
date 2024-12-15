using System;
using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.PlayerProgressUpdaters
{
    public class LevelProgressUpdater : IProgressUpdater
    {
        private int _activeLevel;
        private int _bestScore;
        private List<int> _completeLevelIds;
        private List<int> _unlockedLevelItemIds;

        public Action OnProgressChanged { get; set; }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.ActiveLevel = _activeLevel;
            playerProgress.BestScore = _bestScore;
            playerProgress.CompleteLevelIds = _completeLevelIds;
            playerProgress.UnlockedLevelItemIds = _unlockedLevelItemIds;
        }

        public void OnLoadProgress(PlayerProgress playerProgress)
        {
            _activeLevel = playerProgress.ActiveLevel;
            _bestScore = playerProgress.BestScore;
            _completeLevelIds = playerProgress.CompleteLevelIds;
            _unlockedLevelItemIds = playerProgress.UnlockedLevelItemIds;
        }

        public int GetActiveLevel() =>
            _activeLevel;

        public int GetBestScore() =>
            _bestScore;

        public List<int> GetCompleteLevelIds() =>
            _unlockedLevelItemIds;

        public void ChangeActiveLevel(int level)
        {
            _activeLevel = level;
            OnProgressChanged?.Invoke();
        }

        public void SetCompleteLevel(int level)
        {
            _completeLevelIds.Add(level);
            OnProgressChanged?.Invoke();
        }

        public void UpdateBestScore(int scoreValue)
        {
            _bestScore = scoreValue;
            OnProgressChanged?.Invoke();
        }
    }
}