using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Products;
using Infrastructure.Services.Log;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;
using Infrastructure.Services.RemoteConfig;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;

namespace Infrastructure.Providers.InAppPurchase
{
    public class InAppPurchaseProvider : IDetailedStoreListener
    {
        public Action<string> OnRestoreCompletePurchase;
        
        private readonly RemoteConfigService _remoteConfigService;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly PurchaseProgressUpdater _purchaseProgressUpdater;

        private readonly Dictionary<string, TaskCompletionSource<bool>> _pendingTasks = new();

        private List<InAppProductData> _products;

        private IStoreController _controller;
        private IExtensionProvider _extensions;
        private CrossPlatformValidator _validator;

        public InAppPurchaseProvider(RemoteConfigService remoteConfigService, IExceptionLoggerService exceptionLoggerService, PurchaseProgressUpdater purchaseProgressUpdater)
        {
            _remoteConfigService = remoteConfigService;
            _exceptionLoggerService = exceptionLoggerService;
            _purchaseProgressUpdater = purchaseProgressUpdater;
        }

        public void Initialize()
        {
            #if !UNITY_EDITOR
                _validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
            #endif
            
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            _products = GetProductsData();

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
            
            _purchaseProgressUpdater.SetPendingInAppPurchaseProduct(productId);
            
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
            Debug.LogError(logMessage);
            _exceptionLoggerService.LogError(logMessage);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var logMessage = $"[InAppPurchase] Initialization failed: {error} | {message}";
            Debug.LogError(logMessage);
            _exceptionLoggerService.LogError(logMessage);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var product = purchaseEvent.purchasedProduct;
            var productId = GetProductId(product);
            
            if (!ValidatePurchase(product.receipt))
            {
                _exceptionLoggerService.LogError($"[InAppPurchase] Receipt validation failed for product: {productId}");
                FinishPurchaseTask(productId, false);
                return PurchaseProcessingResult.Complete;
            }
            
            FinishPurchaseTask(productId, true);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            FinishPurchaseTask(GetProductId(product), false);
            
            var logMessage =
                $"[InAppPurchase] PurchaseFailed : {product.definition.id} | failureReason: {failureReason} | transactionId: {product.transactionID}";
            Debug.LogError(logMessage);
            
            _exceptionLoggerService.LogError(logMessage);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            FinishPurchaseTask(GetProductId(product), false);
            
            var logMessage =
                $"[InAppPurchase] PurchaseFailed : {product.definition.id} | failureDescription: {failureDescription} | transactionId: {product.transactionID}";
            Debug.LogError(logMessage);
            _exceptionLoggerService.LogError(logMessage);
        }

        private bool Initialized => _controller != null && _extensions != null;

        private List<InAppProductData> GetProductsData()
        {
            var json = _remoteConfigService.GetValue(RemoteConfigIds.Products);
            if (string.IsNullOrEmpty(json))
            {
                var exceptionText = "[InAppPurchase] Failed to get products data";
                _exceptionLoggerService.LogError(exceptionText);
                throw new Exception(exceptionText);
            }
            
            return JsonConvert.DeserializeObject<List<InAppProductData>>(json);
        }

        private string GetProductId(Product product) =>
            product.definition.id;
        
        private void FinishPurchaseTask(string productId, bool success)
        {
            if (!_pendingTasks.TryGetValue(productId, out var task)) return;

            task.SetResult(success);
            _purchaseProgressUpdater.RemovePendingInAppPurchaseProduct(productId);
            _pendingTasks.Remove(productId);
        }

        private void RestorePendingPurchases()
        {
            foreach (var product in _controller.products.all)
            {
                var productId = product.definition.id;
                
                if (_purchaseProgressUpdater.HasPendingInAppPurchaseProduct(productId))
                {
                    if (product.hasReceipt && ValidatePurchase(product.receipt))
                    {
                        OnRestoreCompletePurchase?.Invoke(productId);
                        _purchaseProgressUpdater.RemovePendingInAppPurchaseProduct(productId);
                    }
                    else
                    {
                        _exceptionLoggerService.LogError($"Invalid or pending purchase for product: {productId}");
                    }
                }
            }
        }

        private bool ValidatePurchase(string receipt)
        {
           #if !UNITY_EDITOR
                 try
            {
                var result = _validator.Validate(receipt);
                foreach (var productReceipt in result)
                {
                    _exceptionLoggerService.Log($"Product ID: {productReceipt.productID}, Purchase Date: {productReceipt.purchaseDate}");
                }
                
                return true;
            }
            catch (IAPSecurityException ex)
            {
                _exceptionLoggerService.LogError($"Invalid receipt: {ex.Message}");
                return false;
            }
           #else
            Debug.Log("[InAppPurchase] ValidatePurchase always returns true in Unity Editor.");
            return true;
            #endif
        }
    }
}