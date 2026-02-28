using System;
using Infrastructure.Controllers.Windows;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Views.UI.Buttons;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class LuckySpinWindow : BaseWindow
    {
        [SerializeField] private WheelView _wheelView;
        [SerializeField] private ButtonWithLabel _freeSpinButton;
        [SerializeField] private ButtonWithLabel _startSpinForRewardedAdButton;
        [SerializeField] private ButtonWithLabel _closeButton;

        private bool _isProcessing;
        
        private LuckySpinWindowModel _windowModel;
        
        public override Type GetWindowControllerType() => typeof(LuckySpinWindowController);

        public override void SetModel(BaseWindowModel model)
        {
            base.SetModel(model);
            _windowModel = windowModel as LuckySpinWindowModel;
        }

        protected override void Draw()
        {
            base.Draw();
            DrawSegments();

            _freeSpinButton.Interactable = _windowModel.CanFreeSpin;
            _startSpinForRewardedAdButton.Interactable = !_windowModel.CanFreeSpin;
        }

        protected override void SubscribeListeners()
        {
            base.SubscribeListeners();
            _closeButton.OnButtonClickAction += OnCloseButtonClicked;
            _freeSpinButton.OnButtonClickAction += _windowModel.OnStartFreeSpinButtonClicked;
            _startSpinForRewardedAdButton.OnButtonClickAction += _windowModel.OnStartSpinForRewardedAdButtonClicked;
            _wheelView.OnCompleteSpinAction += OnCompleteSpin;

            _windowModel.OnStartSpinAction += OnStartSpin;
            _windowModel.RefreshAction += Draw;
        }

        protected override void Clear()
        {
            base.Clear();
            _closeButton.OnButtonClickAction -= OnCloseButtonClicked;
            _freeSpinButton.OnButtonClickAction -= _windowModel.OnStartFreeSpinButtonClicked;
            _startSpinForRewardedAdButton.OnButtonClickAction -= _windowModel.OnStartSpinForRewardedAdButtonClicked;
            _wheelView.OnCompleteSpinAction -= OnCompleteSpin;
            
            _windowModel.OnStartSpinAction -= OnStartSpin;
            _windowModel.RefreshAction -= Draw;
        }

        private void DrawSegments()
        {
            _wheelView.DrawSegments(_windowModel.WheelSegments);
        }

        private void OnCloseButtonClicked()
        {
            if (_isProcessing)
                return;
            
            _windowModel.OnCloseButtonClicked();
        }

        private void OnStartSpin(int segmentIndex)
        {
            if (_isProcessing)
                return;
            
            _isProcessing = true;
            _wheelView.StartSpin(segmentIndex);
        }

        private void OnCompleteSpin(int segmentIndex)
        {
            _isProcessing = false;
            _windowModel.OnCompleteSpin(segmentIndex);
        }
    }
}