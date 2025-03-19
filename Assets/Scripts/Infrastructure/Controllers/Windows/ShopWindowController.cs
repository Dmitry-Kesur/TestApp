using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.Currency;
using Infrastructure.Services.InGamePurchase;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class ShopWindowController : BaseWindowController<ShopWindow>
    {
        private readonly ShopWindowModel _shopWindowModel;
        private readonly StateMachineService _stateMachineService;
        private readonly InGamePurchaseService _inGamePurchaseService;
        private readonly ICurrencyService _currencyService;

        public ShopWindowController(StateMachineService stateMachineService, InGamePurchaseService inGamePurchaseService,
            ICurrencyService currencyService)
        {
            _stateMachineService = stateMachineService;
            _inGamePurchaseService = inGamePurchaseService;
            _currencyService = currencyService;

            _shopWindowModel = new ShopWindowModel(_currencyService);
            _shopWindowModel.SetProducts(_inGamePurchaseService.GetProducts());
            SubscribeListeners();
        }

        public override void OnWindowAdd(BaseWindow view)
        {
            base.OnWindowAdd(view);
            windowView.SetModel(_shopWindowModel);
        }

        private void SubscribeListeners()
        {
            _shopWindowModel.OnBackButtonClickAction = OnBackButtonClick;
        }

        private void OnBackButtonClick() =>
            _stateMachineService.TransitionTo(StateType.MenuState);
    }
}