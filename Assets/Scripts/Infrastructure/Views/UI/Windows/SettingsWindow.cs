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

        public override void Init()
        {
            base.Init();
            itemsSelector.Init(_settingsWindowModel?.GetItems(), _settingsWindowModel.SelectedItemId);
            itemsSelector.OnItemSelectAction = _settingsWindowModel.SelectItem;

            returnButton.OnButtonClickAction = OnReturnButtonClickHandler;
            returnButton.SetButtonText("Return");

            _muteSoundsToggle.OnToggleStateChange = OnChangeMuteSoundsStateChange;
            _muteSoundsToggle.ChangeToggleState(_settingsWindowModel.MuteSounds);
        }

        public override Type GetWindowControllerType() => typeof(SettingsWindowController);

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