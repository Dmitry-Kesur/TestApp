using System.Collections.Generic;
using Infrastructure.Data.Products;
using Infrastructure.Enums;
using Infrastructure.Providers;

namespace Infrastructure.Services
{
    public class InAppPurchaseService : IInAppPurchaseService
    {
        private readonly IInAppPurchaseProvider _purchaseProvider;
        private readonly PurchaseProcessorsResolver _processorsResolver;

        private List<IPurchaseProcessor> _purchaseProcessors;

        public InAppPurchaseService(IInAppPurchaseProvider purchaseProvider, PurchaseProcessorsResolver processorsResolver)
        {
            _purchaseProvider = purchaseProvider;
            _processorsResolver = processorsResolver;
            _purchaseProvider.OnInitializedAction = OnInitialized;
            _purchaseProvider.OnProcessPurchaseAction = OnProcessPurchase;
        }

        public void PurchaseProduct(string productId) =>
            _purchaseProvider.Purchase(productId);

        private void OnInitialized()
        {
            _purchaseProcessors = _processorsResolver.GetProcessors();
        }

        private void OnProcessPurchase(InAppProductData productData)
        {
            var purchaseProcessor = GetPurchaseProcessor(productData.productType);
            purchaseProcessor.ProcessPurchase(productData.productId);
        }

        private IPurchaseProcessor GetPurchaseProcessor(InAppProductType productType) =>
            _purchaseProcessors.Find(processor => processor.ProductType == productType);
    }
}