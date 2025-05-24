using System;
using System.Threading.Tasks;
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

        public InAppPurchaseService(InAppPurchaseProvider purchaseProvider, IAnalyticsService analyticsService, IExceptionLoggerService exceptionLoggerService)
        {
            _purchaseProvider = purchaseProvider;
            _analyticsService = analyticsService;
            _exceptionLoggerService = exceptionLoggerService;
            _purchaseProvider.OnRestoreCompletePurchase += OnRestoreCompletePurchase;
        }

        public Action<string> OnCompletePurchase { get; set; }

        public async Task PurchaseProduct(string productId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productId))
                {
                    _exceptionLoggerService.LogError("PurchaseProduct: productId is null or empty.");
                    return;
                }
                
                var purchaseState = await _purchaseProvider.Purchase(productId);
                if (purchaseState)
                {
                    OnCompletePurchase?.Invoke(productId);
                    _analyticsService.LogCompleteInAppPurchaseProduct(productId);
                    return;
                }
                
                
                _analyticsService.LogFailedInAppPurchaseProduct(productId);
            }
            catch (Exception e)
            {
                _exceptionLoggerService.LogException(e);
            }
        }

        private void OnRestoreCompletePurchase(string productId)
        {
            OnCompletePurchase?.Invoke(productId);
            _analyticsService.LogInAppPurchaseProductRestore(productId);
        }
    }
}