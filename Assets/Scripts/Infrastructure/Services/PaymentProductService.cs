using System;
using Infrastructure.Constants;
using Infrastructure.Data.Notifications;
using Infrastructure.Models.GameEntities.Products;
using Infrastructure.Models.GameEntities.Products.InGame;

namespace Infrastructure.Services
{
    public class PaymentProductService : IPaymentProductService
    {
        private readonly ICurrencyService _currencyService;
        private readonly INotificationService _notificationService;
        private readonly IAdsService _adsService;
        
        public PaymentProductService(ICurrencyService currencyService, INotificationService notificationService, IAdsService adsService)
        {
            _currencyService = currencyService;
            _notificationService = notificationService;
            _adsService = adsService;
        }

        public void PaymentProduct(IProductModel product)
        {
            if (!CanPaymentProduct(product))
            {
                ShowErrorPaymentNotification(product);
                return;
            }
            
            _currencyService.DecreaseCurrency(product.Price);
            OnCompletePaymentProduct?.Invoke(product);
        }

        public Action<IProductModel> OnCompletePaymentProduct { get; set; }

        private void ShowErrorPaymentNotification(IProductModel product)
        {
            var needCurrencyAmount = Math.Abs(product.Price - _currencyService.CurrencyAmount);
                
            var notificationModel = new NotificationWithAdsModel
            {
                NotificationText = $"Need {needCurrencyAmount} currency to buy",
                ShowAdsAction = () => ShowAdsToPaymentProduct(product)
            };
                
            _notificationService.ShowNotification(notificationModel);
        }

        private void ShowAdsToPaymentProduct(IProductModel product)
        {
            _adsService.OnShowCompleteAdsAction = () => OnCompletePaymentProduct?.Invoke(product);
            _adsService.ShowAds(AdsId.Rewarded);
            _notificationService.HideNotification();
        }

        private bool CanPaymentProduct(IProductModel product) =>
            _currencyService.CurrencyAmount >= product.Price;
    }
}