using System;
using Infrastructure.Constants;
using Infrastructure.Models.GameEntities.Shop;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Resource;

namespace Infrastructure.Services.Shop
{
    public class PaymentShopService : IPaymentShopService
    {
        private readonly ResourcesService _resourcesService;
        private readonly INotificationService _notificationService;
        private readonly IAdsService _adsService;
        
        public PaymentShopService(ResourcesService resourcesService, INotificationService notificationService, IAdsService adsService)
        {
            _resourcesService = resourcesService;
            _notificationService = notificationService;
            _adsService = adsService;
        }

        public void PaymentProduct(IShopProductModel iShopProduct)
        {
            if (!CanPaymentProduct(iShopProduct))
            {
                ShowErrorPaymentNotification(iShopProduct);
                return;
            }

            var cost = iShopProduct.Cost;
            _resourcesService.SpendResource(cost.ResourceId, -cost.Amount);
            OnCompletePaymentProduct?.Invoke(iShopProduct);
        }

        public Action<IShopProductModel> OnCompletePaymentProduct { get; set; }

        private void ShowErrorPaymentNotification(IShopProductModel iShopProduct)
        {
            var cost = iShopProduct.Cost;
            var resource = _resourcesService.GetResourceById(cost.ResourceId);
            
            var needCurrencyAmount = Math.Abs(cost.Amount - resource.Amount);
        }

        private void ShowAdsToPaymentProduct(IShopProductModel iShopProduct)
        {
            // _adsService.OnShowCompleteAdsAction = () => OnCompletePaymentProduct?.Invoke(product);
            _adsService.ShowAds(AdsId.Rewarded);
            _notificationService.HideNotification();
        }

        private bool CanPaymentProduct(IShopProductModel iShopProduct)
        {
            var cost = iShopProduct.Cost;
            var resource = _resourcesService.GetResourceById(cost.ResourceId);
            return resource.Amount >= cost.Amount;
        }
    }
}