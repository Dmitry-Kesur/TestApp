using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using Infrastructure.Views.UI.Selectors;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class SettingsWindow : BaseWindow
    {
        [SerializeField] private ButtonWithLabel returnButton;
        [SerializeField] private ItemsSelector itemsSelector;
        [SerializeField] private ToggleButton _muteSoundsToggle;

        private SettingsWindowModel _settingsWindowModel;

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _settingsWindowModel = windowModel as SettingsWindowModel;
        }

        public override Type GetWindowControllerType() => typeof(SettingsWindowController);

        protected override void SubscribeListeners()
        {
            base.SubscribeListeners();
            itemsSelector.OnItemSelectAction = _settingsWindowModel.SelectItem;
            returnButton.OnButtonClickAction = OnReturnButtonClickHandler;
            _muteSoundsToggle.OnToggleStateChange = OnChangeMuteSoundsStateChange;
        }

        protected override void Draw()
        {
            base.Draw();
            itemsSelector.Init(_settingsWindowModel?.GetItems(), _settingsWindowModel.SelectedItemId);
            
            returnButton.SetButtonText("Return");
            
            _muteSoundsToggle.ChangeToggleState(_settingsWindowModel.MuteSounds);
        }

        protected override void Clear()
        {
            base.Clear();
            itemsSelector.OnItemSelectAction = null;
            returnButton.OnButtonClickAction = null;
            _muteSoundsToggle.OnToggleStateChange = null;
        }

        private void OnChangeMuteSoundsStateChange(bool muteSounds)
        {
            _settingsWindowModel.OnChangeMuteSoundsState(muteSounds);
        }

        private void OnReturnButtonClickHandler()
        {
            _settingsWindowModel.OnReturnButtonClick();
        }
    }
}