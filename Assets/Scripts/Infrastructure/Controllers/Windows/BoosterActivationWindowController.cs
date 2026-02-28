using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.Booster;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class BoosterActivationWindowController : BaseWindowController<BoosterActivationWindow>
    {
        private readonly IBoostersService _boostersService;
        private readonly StateMachineService _stateMachineService;
        private BoosterActivationWindowModel _windowModel;

        public BoosterActivationWindowController(IBoostersService boostersService, StateMachineService stateMachineService)
        {
            _boostersService = boostersService;
            _stateMachineService = stateMachineService;
            _windowModel = new BoosterActivationWindowModel();
            _windowModel.CancelAction += OnCancel;
            _boostersService.OnBoosterActivatedAction += OnBoosterActivated;
        }

        protected override void InitParameters()
        {
            base.InitParameters();
            _windowModel.GetBoosterModels = _boostersService.GetAvailableBoosters();
        }

        protected override BaseWindowModel GetModel() => _windowModel;

        private void OnCancel() =>
            _stateMachineService.TransitionTo(StateType.GameLoopState);

        private void OnBoosterActivated() =>
            _stateMachineService.TransitionTo(StateType.GameLoopState);
    }
}