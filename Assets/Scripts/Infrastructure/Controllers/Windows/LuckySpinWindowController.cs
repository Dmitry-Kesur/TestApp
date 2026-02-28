using System;
using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.LuckySpin;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class LuckySpinWindowController : BaseWindowController<LuckySpinWindow>
    {
        private readonly LuckySpinService _luckySpinService;
        private readonly StateMachineService _stateMachineService;
        private readonly LuckySpinWindowModel _windowModel;

        public LuckySpinWindowController(LuckySpinService luckySpinService, StateMachineService stateMachineService)
        {
            _luckySpinService = luckySpinService;
            _luckySpinService.OnStartSpinAction += OnStartSpin;
            _luckySpinService.RefreshAction += Refresh;
            
            _stateMachineService = stateMachineService;
            
            _windowModel = new LuckySpinWindowModel();
            _windowModel.OnCloseButtonClickAction = BackToMenu;
            _windowModel.StartFreeSpinAction = OnStartFreeSpinClicked;
            _windowModel.StartSpinForRewardedAdAction = OnStartSpinForRewardedAdClicked;
            _windowModel.OnCompleteSpinAction = OnCompleteSpin;
        }

        protected override void InitParameters()
        {
            base.InitParameters();
            UpdateParameters();
        }

        private void UpdateParameters()
        {
            _windowModel.WheelSegments = _luckySpinService.SegmentModels;
            _windowModel.CanFreeSpin = _luckySpinService.CanFreeSpin;
        }

        protected override BaseWindowModel GetModel() => _windowModel;

        private void BackToMenu() =>
            _stateMachineService.TransitionTo(StateType.MenuState);

        private void OnStartFreeSpinClicked() =>
            _luckySpinService.StartFreeSpin();

        private void OnStartSpinForRewardedAdClicked() =>
            _luckySpinService.StartSpinForRewardedAd();

        private void OnStartSpin(int segmentIndex) =>
            _windowModel.OnStartSpinAction?.Invoke(segmentIndex);

        private void OnCompleteSpin(int segmentIndex) =>
            _luckySpinService.OnCompleteSpin(segmentIndex);

        private void Refresh()
        {
            UpdateParameters();
            _windowModel.Refresh();
        }
    }
}