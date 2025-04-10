using Infrastructure.Enums;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class SelectLevelState : State
    {
        private readonly IWindowService _windowService;

        public SelectLevelState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.SelectLevelWindow);
        }
    }
}