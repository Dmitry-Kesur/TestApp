using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Products;
using Infrastructure.Models.UI.Items;
using Infrastructure.Providers.InAppPurchase;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Log;
using Infrastructure.Services.Notification;

namespace Infrastructure.Services.InAppPurchase
{
    public class InAppPurchaseService : IInAppPurchaseService, IBootstrapTarget
    {
        private readonly InAppPurchaseProvider _purchaseProvider;
        private readonly IAnalyticsService _analyticsService;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly IInAppProductsSource _inAppProductsSource;
        private readonly INotificationService _notificationService;

        private List<InAppProductModel> _inAppProductModels;

        public InAppPurchaseService(InAppPurchaseProvider purchaseProvider, IAnalyticsService analyticsService,
            IExceptionLoggerService exceptionLoggerService, IInAppProductsSource inAppProductsSource, INotificationService notificationService)
        {
            _purchaseProvider = purchaseProvider;
            _analyticsService = analyticsService;
            _exceptionLoggerService = exceptionLoggerService;
            _inAppProductsSource = inAppProductsSource;
            _notificationService = notificationService;
            _purchaseProvider.OnRestoreCompletePurchase += OnRestoreCompletePurchase;
        }

        public int InitializationOrder => 1;

        public async void Initialize()
        {
            var productsData = await _inAppProductsSource.GetProducts();
            _purchaseProvider.OnInitializedAction = () => CreateProductModels(productsData);
            _purchaseProvider.SetProducts(productsData);
            CreateProductModels(productsData);
        }

        public Action<PurchaseReward> OnCompletePurchaseAction { get; set; }

        public List<InAppProductModel> GetProductModels() => _inAppProductModels;

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
                    OnCompletePurchase(productId);
                    return;
                }


                _analyticsService.LogFailedInAppPurchaseProduct(productId);
            }
            catch (Exception e)
            {
                _exceptionLoggerService.LogException(e);
            }
        }

        private void OnCompletePurchase(string productId)
        {
            var product = GetProductModelById(productId);

            OnCompletePurchaseAction?.Invoke(product.PurchaseReward);
            _analyticsService.LogCompleteInAppPurchaseProduct(productId);
            
            var notificationModel = new NotificationWithIconModel
            {
                NotificationText = "Purchase successful",
                NotificationIcon = product.IconSprite
            };

            _notificationService.ShowNotification(notificationModel);
        }

        private void OnRestoreCompletePurchase(string productId)
        {
            var productModel = GetProductModelById(productId);
            OnCompletePurchaseAction?.Invoke(productModel.PurchaseReward);
            _analyticsService.LogInAppPurchaseProductRestore(productId);
        }
        
        private InAppProductModel GetProductModelById(string productId) =>
            _inAppProductModels.Find(model => model.ProductId == productId);

        private void CreateProductModels(List<InAppProductData> productsData)
        {
            _inAppProductModels = new();
            foreach (var product in productsData)
            {
                var productModel = new InAppProductModel(product);
                productModel.OnPurchaseAction = OnPurchaseProduct;
                productModel.Price = _purchaseProvider.GetLocalizedPriceString(product.productId);
                _inAppProductModels.Add(productModel);
            }
        }

        private async void OnPurchaseProduct(string productId) =>
            await PurchaseProduct(productId);
    }
}