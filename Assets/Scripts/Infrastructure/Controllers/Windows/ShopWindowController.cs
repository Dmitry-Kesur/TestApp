using Infrastructure.Constants;
using Infrastructure.Enums;
using Infrastructure.Models.UI.Windows;
using Infrastructure.Services;
using Infrastructure.Services.Resource;
using Infrastructure.Services.Shop;
using Infrastructure.StateMachine;
using Infrastructure.Views.UI.Windows;

namespace Infrastructure.Controllers.Windows
{
    public class ShopWindowController : BaseWindowController<ShopWindow>
    {
        private readonly ShopWindowModel _shopWindowModel;
        private readonly StateMachineService _stateMachineService;
        private readonly ShopService _shopService;

        public ShopWindowController(StateMachineService stateMachineService, ShopService shopService, ResourcesService resourcesService)
        {
            _stateMachineService = stateMachineService;
            _shopService = shopService;

            var coinResource = resourcesService.GetResourceByType(ResourceType.Coin);
            _shopWindowModel = new ShopWindowModel(coinResource);
            _shopWindowModel.SetProducts(_shopService.GetProducts());
            SubscribeListeners();
        }

        protected override BaseWindowModel GetModel() =>
            _shopWindowModel;

        private void SubscribeListeners()
        {
            _shopWindowModel.OnBackButtonClickAction = OnBackButtonClick;
            _shopService.OnPurchaseCompleted = OnCompletePurchase;
        }

        private void OnCompletePurchase() =>
            _shopWindowModel.OnCompletePurchase();

        private void OnBackButtonClick() =>
            _stateMachineService.TransitionTo(StateType.MenuState);
    }
}