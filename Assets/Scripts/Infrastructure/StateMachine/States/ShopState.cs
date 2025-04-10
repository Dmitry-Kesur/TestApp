using Infrastructure.Enums;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class ShopState : State
    {
        private readonly IWindowService _windowService;

        public ShopState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.ShopWindow);
        }

        public override void Exit()
        {
            _windowService.HideActiveWindow();
        }
    }
}