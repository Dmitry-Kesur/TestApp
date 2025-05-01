using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class WinLevelWindow : BaseWindow
    {
        [SerializeField] private ButtonWithLabel _nextLevelButton;
        [SerializeField] private ButtonWithLabel _menuButton;
        [SerializeField] private TextMeshProUGUI _levelScore;
        
        private CompleteLevelWindowModel _completeLevelWindowModel;

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _completeLevelWindowModel = windowModel as CompleteLevelWindowModel;
        }

        public override Type GetWindowControllerType() =>
            typeof(CompleteLevelWindowController);

        protected override void SubscribeListeners()
        {
            base.SubscribeListeners();
            _nextLevelButton.OnButtonClickAction = OnNextLevelButtonClick;
            _menuButton.OnButtonClickAction = OnMenuButtonClick;
        }

        protected override void Draw()
        {
            base.Draw();
            var levelScore = _completeLevelWindowModel.LevelScore;
            _levelScore.text = levelScore.ToString();

            _nextLevelButton.gameObject.SetActive(_completeLevelWindowModel.CanStartNextLevel);
        }

        protected override void Clear()
        {
            base.Clear();
            _nextLevelButton.OnButtonClickAction = null;
            _menuButton.OnButtonClickAction = null;
        }

        private void OnMenuButtonClick()
        {
            _completeLevelWindowModel.OnMenuButtonClick();
        }

        private void OnNextLevelButtonClick()
        {
            _completeLevelWindowModel.OnNextLevelButtonClick();
        }
    }
}