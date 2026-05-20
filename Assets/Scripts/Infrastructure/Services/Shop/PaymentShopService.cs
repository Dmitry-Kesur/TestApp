using System;
using Infrastructure.Constants;
using Infrastructure.Data.Notifications;
using Infrastructure.Models.GameEntities.Shop;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Resource;

namespace Infrastructure.Services.Shop
{
    public class PaymentShopService : IPaymentShopService
    {
        private readonly ResourcesService _resourcesService;
        private readonly INotificationService _notificationService;
        
        public PaymentShopService(ResourcesService resourcesService, INotificationService notificationService)
        {
            _resourcesService = resourcesService;
            _notificationService = notificationService;
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
            
            var notification = new NotificationWithTextModel
            {
                NotificationText = UIMessages.ShowResourcePurchaseErrorAlias + " " + needCurrencyAmount
            };
            _notificationService.ShowNotification(notification);
        }

        private bool CanPaymentProduct(IShopProductModel iShopProduct)
        {
            var cost = iShopProduct.Cost;
            var resource = _resourcesService.GetResourceById(cost.ResourceId);
            return resource.Amount >= cost.Amount;
        }
    }
}