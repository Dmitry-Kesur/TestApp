using Infrastructure.Enums;
using Infrastructure.Services;

namespace Infrastructure.StateMachine.States
{
    public class BoostersState : State
    {
        private readonly IWindowService _windowService;

        public BoostersState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public override void Enter()
        {
            base.Enter();
            _windowService.ShowWindow(WindowId.BoostersWindow);
        }
    }
}