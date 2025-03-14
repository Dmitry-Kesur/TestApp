using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.PlayerProgressUpdaters
{
    public class LevelProgressUpdater : IProgressUpdater
    {
        public PlayerProgress Progress { get; set; }

        public void OnLoadProgress(PlayerProgress playerProgress)
        {
            Progress = playerProgress;
        }

        public int GetActiveLevel() =>
            Progress.ActiveLevel;

        public int GetBestScore() =>
            Progress.BestScore;

        public List<int> GetCompleteLevelIds() =>
            Progress.UnlockedLevelItemIds;

        public void ChangeActiveLevel(int level)
        {
            Progress.ActiveLevel = level;
        }

        public void SetCompleteLevel(int level)
        {
            Progress.CompleteLevelIds.Add(level);
        }

        public void UpdateBestScore(int scoreValue)
        {
            Progress.BestScore = scoreValue;
        }
    }
}