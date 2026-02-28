using Infrastructure.Enums;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class DailyBonusState : State
    {
        private readonly IWindowService _windowService;

        public DailyBonusState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.DailyBonusWindow);
        }

        public override void Exit()
        {
         _windowService.HideActiveWindow();   
        }
    }
}