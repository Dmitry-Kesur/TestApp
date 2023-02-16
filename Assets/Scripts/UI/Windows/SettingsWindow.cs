using DefaultNamespace;
using UI.Buttons;
using UnityEngine;

namespace UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        [SerializeField] private BaseButton returnButton;
        [SerializeField] private ItemsSelector itemsSelector;

        private SettingsWindowModel _settingsWindowModel;

        public override void Init(BaseWindowModel windowModel)
        {
            base.Init(windowModel);
            _settingsWindowModel = windowModel as SettingsWindowModel;

            itemsSelector.Init(_settingsWindowModel?.GetGameItem());

            returnButton.button.onClick?.AddListener(OnReturnButtonClickHandler);
            
            returnButton.SetButtonText("Return Menu");
        }

        private void OnReturnButtonClickHandler()
        {
            _settingsWindowModel.OnReturnButtonClick();
        }

        public override void OnWindowHide()
        {
            base.OnWindowHide();
            returnButton.button.onClick?.RemoveListener(OnReturnButtonClickHandler);
            itemsSelector.Clear();
        }
    }
}