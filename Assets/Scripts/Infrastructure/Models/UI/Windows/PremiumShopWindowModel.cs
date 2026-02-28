using System;
using System.Collections.Generic;
using Infrastructure.Models.UI.Items;
using Infrastructure.Services.InAppPurchase;

namespace Infrastructure.Models.UI.Windows
{
    public class PremiumShopWindowModel : BaseWindowModel
    {
        public Action OnBackToMenuAction;
        
        private readonly InAppPurchaseService _inAppPurchaseService;

        public PremiumShopWindowModel(InAppPurchaseService inAppPurchaseService)
        {
            _inAppPurchaseService = inAppPurchaseService;
        }

        public List<InAppProductModel> GetProductModels() =>
            _inAppPurchaseService.GetProductModels();

        public void OnBackToMenuButtonClicked() =>
            OnBackToMenuAction?.Invoke();
    }
}