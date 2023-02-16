using DefaultNamespace;
using UI.Buttons;
using UnityEngine;

namespace UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        [SerializeField] private BaseButton selectButton;
        [SerializeField] private BaseButton returnButton;
        [SerializeField] private ItemsSelector itemsSelector;

        private SettingsWindowModel _settingsWindowModel;

        public override void Init(BaseWindowModel windowModel)
        {
            base.Init(windowModel);
            _settingsWindowModel = windowModel as SettingsWindowModel;

            itemsSelector.Init(_settingsWindowModel?.GetItems());

            selectButton.button.onClick?.AddListener(OnSelectButtonClickHandler);
            returnButton.button.onClick?.AddListener(OnReturnButtonClickHandler);
            
            selectButton.SetButtonText("Select");
            returnButton.SetButtonText("Return Menu");
        }
        
        private void OnSelectButtonClickHandler()
        {
            if (itemsSelector.selectedItemId == _settingsWindowModel.selectedItem.id) return;
          
            itemsSelector.AnimateSelectedItem();
            _settingsWindowModel.SelectItem(itemsSelector.selectedItemId);
        }
        
        private void OnReturnButtonClickHandler()
        {
            _settingsWindowModel.OnReturnButtonClick();
        }

        public override void OnWindowHide()
        {
            base.OnWindowHide();
            selectButton.button.onClick?.RemoveListener(OnSelectButtonClickHandler);
            returnButton.button.onClick?.RemoveListener(OnReturnButtonClickHandler);
        }
    }
}