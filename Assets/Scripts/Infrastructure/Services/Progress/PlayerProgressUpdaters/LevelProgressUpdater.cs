using System.Collections.Generic;

namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public class LevelProgressUpdater : ProgressUpdater
    {
        public int GetActiveLevel() =>
            progress.ActiveLevel;

        public int GetBestScore() =>
            progress.BestScore;

        public List<int> GetCompleteLevelIds() =>
            progress.UnlockedLevelItemIds;

        public void ChangeActiveLevel(int level)
        {
            progress.ActiveLevel = level;
        }

        public void SetCompleteLevel(int level)
        {
            progress.CompleteLevelIds.Add(level);
        }

        public void UpdateBestScore(int scoreValue)
        {
            progress.BestScore = scoreValue;
        }
    }
}