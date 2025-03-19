using System;
using Infrastructure.Providers.InAppPurchase;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Log;

namespace Infrastructure.Services.InAppPurchase
{
    public class InAppPurchaseService : IInAppPurchaseService
    {
        private readonly InAppPurchaseProvider _purchaseProvider;
        private readonly IAnalyticsService _analyticsService;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        public InAppPurchaseService(InAppPurchaseProvider purchaseProvider, IAnalyticsService analyticsService)
        {
            _purchaseProvider = purchaseProvider;
            _analyticsService = analyticsService;
            _purchaseProvider.OnRestoreCompletePurchase = OnRestoreCompletePurchase;
        }

        public Action<string> OnCompletePurchase { get; set; }

        public async void PurchaseProduct(string productId)
        {
             var purchaseState = await _purchaseProvider.Purchase(productId);
             if (purchaseState)
             {
                 OnCompletePurchase?.Invoke(productId);
                 _analyticsService.LogInAppPurchaseProduct(productId);
             }
        }

        private void OnRestoreCompletePurchase(string productId)
        {
            OnCompletePurchase?.Invoke(productId);
            _analyticsService.LogInAppPurchaseProductRestore(productId);
        }
    }
}