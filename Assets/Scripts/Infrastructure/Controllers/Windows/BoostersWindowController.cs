using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.Booster;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class BoostersWindowController : BaseWindowController<BoostersWindow>
    {
        private readonly StateMachineService _stateMachineService;
        private readonly IBoostersService _boostersService;
        private readonly BoostersWindowModel _boostersWindowModel;

        public BoostersWindowController(StateMachineService stateMachineService, IBoostersService boostersService)
        {
            _stateMachineService = stateMachineService;
            _boostersService = boostersService;
            _boostersWindowModel = new BoostersWindowModel(_boostersService)
            {
                OnBackToMenuAction = OnBackToMenu
            };
        }

        public override void OnWindowAdd(BaseWindow view)
        {
            base.OnWindowAdd(view);
            windowView.SetModel(_boostersWindowModel);
        }

        private void OnBackToMenu() =>
            _stateMachineService.TransitionTo(StateType.MenuState);
    }
}