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
            
            selectButton.SetButtonText("Select");
            returnButton.SetButtonText("Return Menu");
            
            itemsSelector.Init(_settingsWindowModel?.GetItems());

            selectButton.button.onClick?.AddListener(OnSelectButtonClickHandler);
            returnButton.button.onClick?.AddListener(OnReturnButtonClickHandler);
        }
        
        private void OnSelectButtonClickHandler()
        {
            itemsSelector.AnimateSelectedItem();
            _settingsWindowModel.SelectItem(itemsSelector.selectedItemId);
        }
        
        private void OnReturnButtonClickHandler()
        {
            _settingsWindowModel.OnReturnButtonClick();
        }
    }
}