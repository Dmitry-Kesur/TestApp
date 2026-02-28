using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services.InAppPurchase;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class PremiumShopWindowController : BaseWindowController<PremiumShopWindow>
    {
        private readonly StateMachineService _stateMachineService;
        private readonly PremiumShopWindowModel _premiumShopWindowModel;

        public PremiumShopWindowController(StateMachineService stateMachineService, InAppPurchaseService inAppPurchaseService)
        {
            _stateMachineService = stateMachineService;
            _premiumShopWindowModel = new PremiumShopWindowModel(inAppPurchaseService)
            {
                OnBackToMenuAction = OnBackToMenu
            };
        }

        protected override BaseWindowModel GetModel() =>
            _premiumShopWindowModel;

        private void OnBackToMenu() =>
            _stateMachineService.TransitionTo(StateType.MenuState);
    }
}