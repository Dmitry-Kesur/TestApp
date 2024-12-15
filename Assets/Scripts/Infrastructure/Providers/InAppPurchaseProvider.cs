using System;
using System.Collections.Generic;
using Infrastructure.Data.Products;
using Infrastructure.Services;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Infrastructure.Providers
{
    public class InAppPurchaseProvider : IInAppPurchaseProvider, IDetailedStoreListener
    {
        private readonly IRemoteConfigService _remoteConfigService;

        private List<InAppProductData> _products;

        private IStoreController _controller;
        private IExtensionProvider _extensions;

        public InAppPurchaseProvider(IRemoteConfigService remoteConfigService)
        {
            _remoteConfigService = remoteConfigService;
        }

        public void Initialize()
        {
            ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            _products = GetProductsData();

            foreach (var product in _products)
                builder.AddProduct(product.productId, product.productLifetimeType);

            UnityPurchasing.Initialize(this, builder);

            OnInitializedAction?.Invoke();
        }

        public void Purchase(string productId) =>
            _controller.InitiatePurchase(productId);

        public Action<InAppProductData> OnProcessPurchaseAction { get; set; }

        public Action OnInitializedAction { get; set; }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _controller = controller;
            _extensions = extensions;
        }

        public void OnInitializeFailed(InitializationFailureReason error) =>
            Debug.Log($"[InAppPurchase] Initialization failed: {error}");

        public void OnInitializeFailed(InitializationFailureReason error, string message) =>
            Debug.Log($"[InAppPurchase] Initialization failed: {error} | {message}");

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            var productId = purchaseEvent.purchasedProduct.definition.id;
            Debug.Log($"[InAppPurchase] Purchasing product success: {productId}");

            var productData = GetProductById(productId);
            OnProcessPurchaseAction.Invoke(productData);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) =>
            Debug.Log(
                $"[InAppPurchase] PurchaseFailed : {product.definition.id} | failureReason: {failureReason} | transactionId: {product.transactionID}");

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription) =>
            Debug.Log(
                $"[InAppPurchase] PurchaseFailed : {product.definition.id} | failureDescription: {failureDescription} | transactionId: {product.transactionID}");

        private InAppProductData GetProductById(string productId) =>
            _products.Find(data => data.productId == productId);

        private List<InAppProductData> GetProductsData()
        {
            var json = _remoteConfigService.GetValue("Products");
            return JsonConvert.DeserializeObject<List<InAppProductData>>(json);
        }
    }
}