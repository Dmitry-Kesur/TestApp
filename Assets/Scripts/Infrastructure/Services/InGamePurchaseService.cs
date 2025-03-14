﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Preloader;
using Infrastructure.Data.Products;
using Infrastructure.Factories;
using Infrastructure.Models.GameEntities.Products.InGame;
using Infrastructure.Services.PlayerProgressUpdaters;
using Infrastructure.Strategy;

namespace Infrastructure.Services
{
    public class InGamePurchaseService : IInGamePurchaseService, ILoadableService
    {
        private readonly List<ProductModel> _shopProducts = new();

        private readonly LocalAddressableService _localAddressableService;
        private readonly IPaymentProductService _paymentProductService;
        private readonly IAnalyticsService _analyticsService;
        private readonly ResourceProgressUpdater _resourceProgressUpdater;
        private readonly INotificationService _notificationService;
        private readonly IProductStrategiesFactory _productStrategiesFactory;

        private List<IProductStrategy> _productStrategies;

        public InGamePurchaseService(LocalAddressableService localAddressableService,
            IPaymentProductService paymentProductService, IAnalyticsService analyticsService,
            ResourceProgressUpdater resourceProgressUpdater, INotificationService notificationService,
            IProductStrategiesFactory productStrategiesFactory)
        {
            _localAddressableService = localAddressableService;
            _paymentProductService = paymentProductService;
            _analyticsService = analyticsService;
            _resourceProgressUpdater = resourceProgressUpdater;
            _notificationService = notificationService;
            _productStrategiesFactory = productStrategiesFactory;

            SubscribeListeners();
        }

        public List<ProductModel> GetProducts() =>
            _shopProducts;

        public LoadingStage LoadingStage => 
            LoadingStage.LoadingProducts;

        public async Task Load()
        {
            _productStrategies = _productStrategiesFactory.CreateProductStrategies();
            var products = await LoadProductsData();
            CreateProducts(products);
        }

        private void CreateProducts(List<ProductData> productsData)
        {
            foreach (var productsStrategy in _productStrategies)
            {
                var products = productsStrategy.CreateProducts(productsData);
                _shopProducts.AddRange(products);
            }

            UpdatePurchasedProducts();
            SubscribeListeners();
        }

        private void UpdatePurchasedProducts()
        {
            var purchasedProductIds = _resourceProgressUpdater.GetPurchasedProductIds();

            foreach (var productId in purchasedProductIds)
            {
                var productById = _shopProducts.Find(model => model.Id == productId);
                productById.Purchased = true;
            }
        }

        private void OnPurchaseProduct(ProductModel product) =>
            _paymentProductService.PaymentProduct(product);

        private void OnCompletePurchaseProduct(IProductModel product)
        {
            product.OnPurchaseComplete();
            var productId = product.Id;
            _resourceProgressUpdater.SetPurchasedProductId(productId);
            _analyticsService.LogPurchaseProduct(productId);

            ShowPurchaseProductNotification(product);
        }

        private void ShowPurchaseProductNotification(IProductModel product)
        {
            var notificationModel = new NotificationWithIconModel
            {
                NotificationText = TextAliases.SuccessfulPurchaseAlias,
                NotificationIcon = product.ProductIcon
            };

            _notificationService.ShowNotification(notificationModel);
        }

        private void SubscribeListeners()
        {
            foreach (var shopProduct in _shopProducts)
            {
                shopProduct.OnPurchaseProductAction = OnPurchaseProduct;
            }

            _paymentProductService.OnCompletePaymentProduct = OnCompletePurchaseProduct;
        }

        private async Task<List<ProductData>> LoadProductsData()
        {
            var products =
                await _localAddressableService.LoadScriptableCollectionFromGroupAsync<ProductData>(AddressableGroupNames
                    .ProductsGroup);
            return products;
        }
    }
}