using Infrastructure.Enums;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class LuckySpinState : State
    {
        private readonly IWindowService _windowService;

        public LuckySpinState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.LuckySpinWindow);
        }

        public override void Exit()
        {
            _windowService.HideActiveWindow();
        }
    }
}