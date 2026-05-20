using Infrastructure.Enums;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class PauseGameLoopState : State
    {
        private readonly IWindowService _windowService;

        public PauseGameLoopState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.PauseGameWindow);
        }

        public override void Exit()
        {
            _windowService.HideActiveWindow();
        }
    }
}