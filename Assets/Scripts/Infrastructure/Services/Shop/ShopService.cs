using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Notifications;
using Infrastructure.Data.Preloader;
using Infrastructure.Data.Products;
using Infrastructure.Factories.Shop;
using Infrastructure.Models.GameEntities.Shop;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Progress;
using Infrastructure.Services.Resource;

namespace Infrastructure.Services.Shop
{
    public class ShopService : ILoadableService, IBootstrapTarget
    {
        private readonly List<ShopProductModel> _productModels = new();

        private readonly LocalAddressableService _localAddressableService;
        private readonly IPaymentShopService _paymentProductService;
        private readonly IAnalyticsService _analyticsService;
        private readonly ISaveLoadProgressService _saveLoadProgressService;
        private readonly INotificationService _notificationService;
        private readonly ShopProductFactory _productFactory;
        private readonly ResourcesService _resourcesService;
        private readonly ShopProductRewardResolver _productRewardResolver;

        public Action OnPurchaseCompleted;
        
        private List<ShopProductData> _productsData;

        public ShopService(LocalAddressableService localAddressableService,
            IPaymentShopService paymentProductService, IAnalyticsService analyticsService,
            ISaveLoadProgressService saveLoadProgressService, INotificationService notificationService,
            ShopProductFactory productFactory, ResourcesService resourcesService, ShopProductRewardResolver productRewardResolver)
        {
            _localAddressableService = localAddressableService;
            _paymentProductService = paymentProductService;
            _analyticsService = analyticsService;
            _saveLoadProgressService = saveLoadProgressService;
            _notificationService = notificationService;
            _productFactory = productFactory;
            _resourcesService = resourcesService;
            _productRewardResolver = productRewardResolver;
        }

        public IReadOnlyList<ShopProductModel> GetProducts() =>
            _productModels;

        public LoadingStage LoadingStage =>
            LoadingStage.LoadingShop;

        public async Task Load()
        {
            _productsData = await _localAddressableService.LoadScriptableCollectionFromGroupAsync<ShopProductData>(
                AddressableGroupNames
                    .ProductsGroup);
        }

        public int InitializationOrder => 3;

        public void Initialize()
        {
            CreateProducts(_productsData);
            UpdatePurchasedProducts();
        }

        private void CreateProducts(List<ShopProductData> productsData)
        {
            foreach (var productData in productsData)
            {
                var productModel = _productFactory.CreateProductModel(productData);
                _productModels.Add(productModel);
            }

            SubscribeListeners();
        }

        private void UpdatePurchasedProducts()
        {
            var purchasedProductIds = _saveLoadProgressService.Read(progress => progress.PurchasedShopProductIds);

            foreach (var productId in purchasedProductIds)
            {
                var productById = _productModels.Find(model => model.Id == productId);
                if (productById == null)
                    continue;
                
                productById.Purchased = true;
            }
        }

        private void OnPurchaseProduct(ShopProductModel shopProduct)
        {
            if (shopProduct.PurchaseType == ShopProductPurchaseType.NonConsumable)
            {
                var purchasedProducts = _saveLoadProgressService.Read(progress => progress.PurchasedShopProductIds);
                if (purchasedProducts.Contains(shopProduct.Id))
                {
                    var notificationModel = new NotificationWithTextModel
                    {
                        NotificationText = UIMessages.ProductAlreadyPurchasedAlias
                    };

                    _notificationService.ShowNotification(notificationModel);
                    return;
                }
            }
            
            _paymentProductService.PaymentProduct(shopProduct);
        }

        private void OnCompletePurchaseProduct(IShopProductModel product)
        {
            _productRewardResolver.Resolve(product);

            if (product.PurchaseType == ShopProductPurchaseType.NonConsumable)
            {
                product.Purchased = true;
                _saveLoadProgressService.Write(progress => progress.AddPurchasedShopProduct(product.Id));
            }
           
            _analyticsService.LogPurchaseInGameProduct(product.Id);

            ShowPurchaseProductNotification(product);
            
            OnPurchaseCompleted?.Invoke();
        }

        private void ShowPurchaseProductNotification(IShopProductModel iShopProduct)
        {
            var notificationModel = new NotificationWithIconModel
            {
                NotificationText = UIMessages.SuccessfulPurchaseAlias,
                NotificationIcon = iShopProduct.ProductIcon
            };

            _notificationService.ShowNotification(notificationModel);
        }

        private void SubscribeListeners()
        {
            foreach (var shopProduct in _productModels)
            {
                shopProduct.OnPurchaseProductAction = OnPurchaseProduct;
            }

            _paymentProductService.OnCompletePaymentProduct = OnCompletePurchaseProduct;
        }
    }
}