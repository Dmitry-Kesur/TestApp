using System;
using Infrastructure.Constants;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class MenuWindow : BaseWindow
    {
        [SerializeField] private ButtonWithLabel _playButton;
        [SerializeField] private ButtonWithLabel _settingsButton;
        [SerializeField] private ButtonWithLabel _boostersButton;
        [SerializeField] private ButtonWithIcon _shopButton;
        [SerializeField] private TextMeshProUGUI _scoreTitle;
        [SerializeField] private TextMeshProUGUI _bestScoreTextField;

        private MenuWindowModel _menuWindowModel;

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _menuWindowModel = windowModel as MenuWindowModel;
        }

        public override Type GetWindowControllerType() => typeof(MenuWindowController);

        protected override void SubscribeListeners()
        {
            base.SubscribeListeners();
            _playButton.OnButtonClickAction = _menuWindowModel.PlayButtonClick;
            _settingsButton.OnButtonClickAction = _menuWindowModel.OnSettingsButtonClick;
            _boostersButton.OnButtonClickAction = _menuWindowModel.OnBoostersButtonClick;
            _shopButton.OnButtonClickAction = _menuWindowModel.OnShopButtonClick;
        }

        protected override void Draw()
        {
            base.Draw();
            _bestScoreTextField.text = _menuWindowModel?.BestScore.ToString();
            _scoreTitle.text = UIMessages.BestScoreAlias;

            _playButton.SetButtonText(UIMessages.PlayGameAlias);
            _settingsButton.SetButtonText(UIMessages.SettingsAlias);
        }

        protected override void Clear()
        {
            base.Clear();
            _playButton.OnButtonClickAction = null;
            _settingsButton.OnButtonClickAction = null;
            _boostersButton.OnButtonClickAction = null;
            _shopButton.OnButtonClickAction = null;
        }
    }
}