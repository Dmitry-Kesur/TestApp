using System;
using Infrastructure.Controllers;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Windows
{
    public class PreloaderWindow : BaseWindow
    {
        private const float MaxProgressValue = 1;
        
        [SerializeField] private ButtonWithIcon _startGameButton;
        [SerializeField] private Image _fill;
        [SerializeField] private TextMeshProUGUI _loadingProgressTextField;
        
        private PreloaderWindowModel _preloaderWindowModel;

        public override void Init()
        {
            base.Init();
            _preloaderWindowModel.OnUpdateLoadingProgressAction = OnUpdateLoadingProgress;
            _startGameButton.OnButtonClickAction = _preloaderWindowModel.StartGame;
            _startGameButton.gameObject.SetActive(false);
            _loadingProgressTextField.gameObject.SetActive(true);
        }

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _preloaderWindowModel = model as PreloaderWindowModel;
        }

        public override Type GetWindowControllerType() => typeof(PreloaderWindowController);

        protected override void Clear()
        {
            base.Clear();
            _preloaderWindowModel.OnUpdateLoadingProgressAction = null;
            _startGameButton.OnButtonClickAction = null;

            _startGameButton.gameObject.SetActive(false);
        }

        private void OnUpdateLoadingProgress(float progress, string stageText)
        {
            _loadingProgressTextField.text = stageText;
            _fill.fillAmount = progress;

            if (progress >= MaxProgressValue)
            {
                _loadingProgressTextField.gameObject.SetActive(false);
                _startGameButton.gameObject.SetActive(true);
            }
        }
    }
}