using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data.Products;
using Infrastructure.Services.InAppPurchase;
using Infrastructure.Services.Log;
using Infrastructure.Services.Progress;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Infrastructure.Providers.InAppPurchase
{
    public class InAppPurchaseProvider : IDetailedStoreListener
    {
        public Action OnInitializedAction;
        public Action<string> OnRestoreCompletePurchase;

        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly IPurchaseValidator _purchaseValidator;

        private readonly Dictionary<string, TaskCompletionSource<bool>> _pendingTasks = new();

        private List<InAppProductData> _products;

        private IStoreController _controller;
        private IExtensionProvider _extensions;

        public InAppPurchaseProvider(IExceptionLoggerService exceptionLoggerService,
            ISaveLoadProgressService saveLoadProgressService,
            IPurchaseValidator purchaseValidator)
        {
            _exceptionLoggerService = exceptionLoggerService;
            _saveLoadProgressService = saveLoadProgressService;
            _purchaseValidator = purchaseValidator;
        }

        public void SetProducts(List<InAppProductData> products)
        {
            _products = products;
            AfterSetProducts();
        }

        public async Task<bool> Purchase(string productId)
        {
            if (!Initialized)
                return false;

            if (_pendingTasks.ContainsKey(productId))
            {
                _exceptionLoggerService.LogError("Purchase already in progress, productId: " + productId);
                return false;
            }

            var tcs = new TaskCompletionSource<bool>();
            _pendingTasks[productId] = tcs;

            _saveLoadProgressService.Write(progress => progress.MarkProductAsPending(productId));

            _controller.InitiatePurchase(productId);

            return await tcs.Task;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;

            OnInitializedAction?.Invoke();
            RestorePendingPurchases();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            var logMessage = $"[InAppPurchase] Initialization failed: {error}";
            _exceptionLoggerService.LogError(logMessage);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var logMessage = $"[InAppPurchase] Initialization failed: {error} | {message}";
            _exceptionLoggerService.LogError(logMessage);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;
            var productId = GetProductId(product);

            if (!_purchaseValidator.Validate(product.receipt))
            {
                _exceptionLoggerService.LogError($"[InAppPurchase] Receipt validation failed for product: {productId}");
                ResolvePendingPurchase(productId, false);
                return PurchaseProcessingResult.Complete;
            }

            ResolvePendingPurchase(productId, true);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            ResolvePendingPurchase(GetProductId(product), false);

            var logMessage =
                $"[InAppPurchase] PurchaseFailed : {product.definition.id} | failureReason: {failureReason} | transactionId: {product.transactionID}";

            _exceptionLoggerService.LogError(logMessage);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            ResolvePendingPurchase(GetProductId(product), false);

            var logMessage =
                $"[InAppPurchase] PurchaseFailed : {product.definition.id} | failureDescription: {failureDescription} | transactionId: {product.transactionID}";
            _exceptionLoggerService.LogError(logMessage);
        }
        
        public string GetLocalizedPriceString(string productId)
        {
            var product = _controller?.products?.WithID(productId);
            return product?.metadata?.localizedPriceString;
        }

        private bool Initialized => _controller != null && _extensions != null;

        private string GetProductId(Product product) =>
            product.definition.id;

        private void ResolvePendingPurchase(string productId, bool success)
        {
            if (!_pendingTasks.TryGetValue(productId, out var task)) return;

            task.SetResult(success);
            _saveLoadProgressService.Write(progress => progress.RemoveProductFromPending(productId));
            _pendingTasks.Remove(productId);
        }

        private void RestorePendingPurchases()
        {
            foreach (var product in _controller.products.all)
            {
                var productId = product.definition.id;
                bool isPending =
                    _saveLoadProgressService.Read(progress => progress.PendingInAppProducts.Contains(productId));

                if (isPending)
                {
                    #if UNITY_EDITOR
                        _saveLoadProgressService.Write(progress => progress.RemoveProductFromPending(productId));
                        OnRestoreCompletePurchase?.Invoke(productId);
                        return;
                    #endif
                    
                    if (product.hasReceipt && _purchaseValidator.Validate(product.receipt))
                    {
                        _saveLoadProgressService.Write(progress => progress.RemoveProductFromPending(productId));
                        OnRestoreCompletePurchase?.Invoke(productId);
                    }
                    else
                    {
                        _exceptionLoggerService.LogError($"Invalid or pending purchase for product: {productId}");
                    }
                }
            }
        }

        private void AfterSetProducts()
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (var product in _products)
                builder.AddProduct(product.productId, product.productLifetimeType);

            UnityPurchasing.Initialize(this, builder);
        }
    }
}