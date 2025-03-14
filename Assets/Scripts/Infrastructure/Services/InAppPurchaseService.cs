using Infrastructure.Data.Products;
using Infrastructure.Providers;

namespace Infrastructure.Services
{
    public class InAppPurchaseService : IInAppPurchaseService
    {
        private readonly InAppPurchaseProvider _purchaseProvider;
        private readonly PurchaseProcessorsProvider _purchaseProcessorsProvider;

        public InAppPurchaseService(InAppPurchaseProvider purchaseProvider, PurchaseProcessorsProvider purchaseProcessorsProvider)
        {
            _purchaseProvider = purchaseProvider;
            _purchaseProcessorsProvider = purchaseProcessorsProvider;
            _purchaseProvider.OnProcessPurchaseAction = OnProcessPurchase;
        }

        public void PurchaseProduct(string productId) =>
            _purchaseProvider.Purchase(productId);

        private void OnProcessPurchase(InAppProductData productData)
        {
            var purchaseProcessor = _purchaseProcessorsProvider.GetPurchaseProcessor(productData.productType);
            purchaseProcessor.ProcessPurchase(productData.productId);
        }
    }
}