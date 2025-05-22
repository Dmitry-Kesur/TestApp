using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Products;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.InAppPurchase;
using Infrastructure.Services.Log;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;
using Infrastructure.Services.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Infrastructure.Providers.InAppPurchase
{
    public class InAppPurchaseProvider : IDetailedStoreListener, IBootstrapTarget
    {
        public Action<string> OnRestoreCompletePurchase;

        private readonly RemoteConfigService _remoteConfigService;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly PurchaseProgressUpdater _progressUpdater;
        private readonly IPurchaseValidator _purchaseValidator;

        private readonly Dictionary<string, TaskCompletionSource<bool>> _pendingTasks = new();

        private List<InAppProductData> _products;

        private IStoreController _controller;
        private IExtensionProvider _extensions;

        public InAppPurchaseProvider(RemoteConfigService remoteConfigService,
            IExceptionLoggerService exceptionLoggerService, PurchaseProgressUpdater progressUpdater,
            IPurchaseValidator purchaseValidator)
        {
            _remoteConfigService = remoteConfigService;
            _exceptionLoggerService = exceptionLoggerService;
            _progressUpdater = progressUpdater;
            _purchaseValidator = purchaseValidator;
        }

        public int InitializationOrder => 1;

        public void Initialize()
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            _products = LoadProductsFromConfig();

            foreach (var product in _products)
                builder.AddProduct(product.productId, product.productLifetimeType);

            UnityPurchasing.Initialize(this, builder);
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

            _progressUpdater.MarkProductAsPending(productId);

            _controller.InitiatePurchase(productId);

            return await tcs.Task;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;

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

        private bool Initialized => _controller != null && _extensions != null;

        private List<InAppProductData> LoadProductsFromConfig()
        {
            var json = _remoteConfigService.GetValue(RemoteConfigIds.Products);
            if (string.IsNullOrEmpty(json))
            {
                var exceptionText = "[InAppPurchase] Failed to get products data";
                _exceptionLoggerService.LogError(exceptionText);
                throw new InvalidOperationException(exceptionText);
            }

            return JsonConvert.DeserializeObject<List<InAppProductData>>(json);
        }

        private string GetProductId(Product product) =>
            product.definition.id;

        private void ResolvePendingPurchase(string productId, bool success)
        {
            if (!_pendingTasks.TryGetValue(productId, out var task)) return;

            task.SetResult(success);
            _progressUpdater.RemovePendingProduct(productId);
            _pendingTasks.Remove(productId);
        }

        private void RestorePendingPurchases()
        {
            foreach (var product in _controller.products.all)
            {
                var productId = product.definition.id;

                if (_progressUpdater.CheckPending(productId))
                {
                    if (product.hasReceipt && _purchaseValidator.Validate(product.receipt))
                    {
                        OnRestoreCompletePurchase?.Invoke(productId);
                        _progressUpdater.RemovePendingProduct(productId);
                    }
                    else
                    {
                        _exceptionLoggerService.LogError($"Invalid or pending purchase for product: {productId}");
                    }
                }
            }
        }
    }
}