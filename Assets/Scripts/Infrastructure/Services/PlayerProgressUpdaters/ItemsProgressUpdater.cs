using System;
using System.Collections.Generic;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.PlayerProgressUpdaters
{
    public class ItemsProgressUpdater : IProgressUpdater
    {
        private int _selectedItemId;
        private List<int> _unlockedLevelItemIds;

        public Action OnProgressChanged { get; set; }

        public void UpdateProgress(PlayerProgress playerProgress)
        {
            playerProgress.SelectedItemId = _selectedItemId;
            playerProgress.UnlockedLevelItemIds = _unlockedLevelItemIds;
        }

        public void OnLoadProgress(PlayerProgress playerProgress)
        {
            _selectedItemId = playerProgress.SelectedItemId;
            _unlockedLevelItemIds = playerProgress.UnlockedLevelItemIds;
        }

        public int GetSelectedItemId() =>
            _selectedItemId;

        public List<int> GetUnlockedItemIds() =>
            _unlockedLevelItemIds;

        public void SetSelectedItemId(int itemId)
        {
            _selectedItemId = itemId;
            OnProgressChanged?.Invoke();
        }

        public void SetUnlockedItem(int itemId)
        {
            _unlockedLevelItemIds.Add(itemId);
            OnProgressChanged?.Invoke();
        }
    }
}