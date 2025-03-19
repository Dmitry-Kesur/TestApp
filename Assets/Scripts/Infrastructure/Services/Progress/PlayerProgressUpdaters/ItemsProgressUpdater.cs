using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public class ItemsProgressUpdater : ProgressUpdater
    {
        public int GetSelectedItemId() =>
            progress.SelectedItemId;

        public List<int> GetUnlockedItemIds() =>
            progress.UnlockedLevelItemIds;

        public void SetSelectedItemId(int itemId)
        {
            progress.SelectedItemId = itemId;
        }

        public void SetUnlockedItem(int itemId)
        {
            progress.UnlockedLevelItemIds.Add(itemId);
        }
    }
}