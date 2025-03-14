using System;
using System.Collections.Generic;
using Infrastructure.Constants;
using Infrastructure.Data.Products;
using Infrastructure.Services;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Infrastructure.Providers
{
    public class InAppPurchaseProvider : IDetailedStoreListener
    {
        private readonly RemoteConfigService _remoteConfigService;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        private List<InAppProductData> _products;

        private IStoreController _controller;
        private IExtensionProvider _extensions;

        public InAppPurchaseProvider(RemoteConfigService remoteConfigService, IExceptionLoggerService exceptionLoggerService)
        {
            _remoteConfigService = remoteConfigService;
            _exceptionLoggerService = exceptionLoggerService;
        }

        public void Initialize()
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            _products = GetProductsData();

            foreach (var product in _products)
                builder.AddProduct(product.productId, product.productLifetimeType);

            UnityPurchasing.Initialize(this, builder);
        }

        public void Purchase(string productId) =>
            _controller.InitiatePurchase(productId);

        public Action<InAppProductData> OnProcessPurchaseAction { get; set; }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            var logMessage = "[InAppPurchase] Initialization failed:" + " " + error;
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
            var productId = purchaseEvent.purchasedProduct.definition.id;
            Debug.Log($"[InAppPurchase] Purchasing product success: {productId}");

            var productData = GetProductById(productId);
            OnProcessPurchaseAction.Invoke(productData);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            var logMessage =
                $"[InAppPurchase] PurchaseFailed : {product.definition.id} | failureReason: {failureReason} | transactionId: {product.transactionID}";
            Debug.LogError(logMessage);
            _exceptionLoggerService.LogError(logMessage);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            var logMessage =
                $"[InAppPurchase] PurchaseFailed : {product.definition.id} | failureDescription: {failureDescription} | transactionId: {product.transactionID}";
            Debug.LogError(logMessage);
            _exceptionLoggerService.LogError(logMessage);
        }

        private InAppProductData GetProductById(string productId) =>
            _products.Find(data => data.productId == productId);

        private List<InAppProductData> GetProductsData()
        {
            var json = _remoteConfigService.GetValue(RemoteConfigKeys.Products);
            if (string.IsNullOrEmpty(json))
            {
                var exceptionText = "[InAppPurchase] Failed to get products data";
                _exceptionLoggerService.LogError(exceptionText);
                throw new Exception(exceptionText);
            }
            
            return JsonConvert.DeserializeObject<List<InAppProductData>>(json);
        }
    }
}