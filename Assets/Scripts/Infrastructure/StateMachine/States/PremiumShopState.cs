using Infrastructure.Enums;
using Infrastructure.Services.Window;

namespace Infrastructure.StateMachine.States
{
    public class PremiumShopState : State
    {
        private readonly IWindowService _windowService;

        public PremiumShopState(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public override void Enter()
        {
            _windowService.ShowWindow(WindowId.PremiumShopWindow);
        }
    }
}