using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using Infrastructure.Views.UI.Loaders;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class SelectLevelWindow : BaseWindow
    {
        [SerializeField] private ButtonWithLabel _backButton;
        [SerializeField] private LevelsLoader _levelsLoader;

        private SelectLevelWindowModel _selectLevelWindowModel;

        public override void Init()
        {
            base.Init();

            _backButton.OnButtonClickAction = OnBackButtonClick;
            _levelsLoader.DrawLoader(_selectLevelWindowModel.GetLevelPreviews());
            _levelsLoader.OnLevelSelectAction = _selectLevelWindowModel.OnLevelSelect;
        }

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _selectLevelWindowModel = model as SelectLevelWindowModel;
        }

        public override Type GetWindowControllerType() =>
            typeof(SelectLevelWindowController);

        protected override void Clear()
        {
            base.Clear();
            _backButton.OnButtonClickAction = null;
            _levelsLoader.OnLevelSelectAction = null;
        }

        private void OnBackButtonClick()
        {
            _selectLevelWindowModel.OnBackButtonClick();
        }
    }
}