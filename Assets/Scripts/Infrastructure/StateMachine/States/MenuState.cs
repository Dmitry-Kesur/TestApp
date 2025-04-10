using Infrastructure.Enums;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class MenuState : State
    {
        private readonly IWindowService _windowService;

        public MenuState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.MenuWindow);
        }

        public override void Exit()
        {
            _windowService.HideWindow(WindowId.MenuWindow);
        }
    }
}