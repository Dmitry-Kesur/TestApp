using Infrastructure.Constants;
using Infrastructure.Enums;
using Infrastructure.Services;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class SettingsState : State
    {
        private readonly IWindowService _windowService;

        public SettingsState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.SettingsWindow);
        }

        public override void Exit()
        {
            _windowService.HideWindow(WindowId.SettingsWindow);
        }
    }
}