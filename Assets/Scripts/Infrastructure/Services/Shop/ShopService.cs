using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Preloader;
using Infrastructure.Data.Products;
using Infrastructure.Factories.Purchase;
using Infrastructure.Models.GameEntities.Products.InGame;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Progress.PlayerProgressUpdaters;
using Infrastructure.Strategy;

namespace Infrastructure.Services.InGamePurchase
{
    public class ShopService : ILoadableService
    {
        private readonly List<ProductModel> _shopProducts = new();

        private readonly LocalAddressableService _localAddressableService;
        private readonly IPaymentShopService _paymentProductService;
        private readonly IAnalyticsService _analyticsService;
        private readonly PurchaseProgressUpdater _purchaseProgressUpdater;
        private readonly INotificationService _notificationService;
        private readonly IShopProductStrategiesFactory _productStrategiesFactory;

        private List<IProductStrategy> _productStrategies;
        private List<ProductData> _products;

        public ShopService(LocalAddressableService localAddressableService,
            IPaymentShopService paymentProductService, IAnalyticsService analyticsService,
            PurchaseProgressUpdater purchaseProgressUpdater, INotificationService notificationService,
            IShopProductStrategiesFactory productStrategiesFactory)
        {
            _localAddressableService = localAddressableService;
            _paymentProductService = paymentProductService;
            _analyticsService = analyticsService;
            _purchaseProgressUpdater = purchaseProgressUpdater;
            _notificationService = notificationService;
            _productStrategiesFactory = productStrategiesFactory;

            SubscribeListeners();
        }

        public List<ProductModel> GetProducts() =>
            _shopProducts;

        public LoadingStage LoadingStage => 
            LoadingStage.LoadingShopProducts;

        public async Task Load()
        {
            _products = await _localAddressableService.LoadScriptableCollectionFromGroupAsync<ProductData>(AddressableGroupNames
                    .ProductsGroup);
        }

        public void Initialize()
        {
            _productStrategies = _productStrategiesFactory.CreateProductStrategies();
            CreateProducts(_products);
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
            var purchasedProductIds = _purchaseProgressUpdater.GetPurchasedShopProductIds();

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
            HandleProductPurchaseCompletion(product);
            _purchaseProgressUpdater.SetPurchasedShopProductId(product.Id);
            _analyticsService.LogPurchaseProduct(product.Id);

            ShowPurchaseProductNotification(product);
        }

        private static void HandleProductPurchaseCompletion(IProductModel product)
        {
            var purchaseTarget = product.GetPurchaseTarget();
            purchaseTarget.OnPurchaseComplete();
            product.UpdateProductAction?.Invoke();
        }

        private void ShowPurchaseProductNotification(IProductModel product)
        {
            var notificationModel = new NotificationWithIconModel
            {
                NotificationText = UIMessages.SuccessfulPurchaseAlias,
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
    }
}