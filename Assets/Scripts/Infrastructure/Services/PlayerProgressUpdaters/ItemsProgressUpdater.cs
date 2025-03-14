using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.PlayerProgressUpdaters
{
    public class ItemsProgressUpdater : IProgressUpdater
    {
        public PlayerProgress Progress { get; set; }

        public void OnLoadProgress(PlayerProgress playerProgress)
        {
            Progress = playerProgress;
        }

        public int GetSelectedItemId() =>
            Progress.SelectedItemId;

        public List<int> GetUnlockedItemIds() =>
            Progress.UnlockedLevelItemIds;

        public void SetSelectedItemId(int itemId)
        {
            Progress.SelectedItemId = itemId;
        }

        public void SetUnlockedItem(int itemId)
        {
            Progress.UnlockedLevelItemIds.Add(itemId);
        }
    }
}