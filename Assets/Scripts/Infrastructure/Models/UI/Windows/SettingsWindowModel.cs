using System;
using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Level.Items;

namespace Infrastructure.Models.UI.Windows
{
    public class SettingsWindowModel : BaseWindowModel
    {
        public Action<int> OnItemSelectAction;
        public Action<bool> OnChangeMuteSoundsStateAction;
        public Action OnReturnAction;

        private int _selectedItemId;
        private List<ItemModel> _items;

        public void SetItems(List<ItemModel> items)
        {
            _items = items;
        }

        public List<ItemModel> GetItems() => _items;

        public int SelectedItemId
        {
            get => _selectedItemId;
            set => _selectedItemId = value;
        }
        
        public bool MuteSounds { get; set; }
        
        public void OnReturnButtonClick()
        {
            OnReturnAction?.Invoke();
        }

        public void SelectItem(int itemId)
        {
            SelectedItemId = itemId;
            OnItemSelectAction?.Invoke(itemId);
        }

        public void OnChangeMuteSoundsState(bool muteSounds)
        {
            OnChangeMuteSoundsStateAction?.Invoke(muteSounds);
        }
    }
}