using Infrastructure.Enums;
using Infrastructure.Services;

namespace Infrastructure.StateMachine.States
{
    public class AuthenticationState : State
    {
        private readonly IWindowService _windowService;

        public AuthenticationState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.AuthenticationWindow);
        }

        public override void Exit()
        {
            _windowService.HideWindow(WindowId.AuthenticationWindow);
        }
    }
}