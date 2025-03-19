using System;
using Infrastructure.Models.GameEntities.Products.InGame;

namespace Infrastructure.Services.InGamePurchase
{
    public interface IPaymentProductService
    {
        void PaymentProduct(IProductModel product);
        Action<IProductModel> OnCompletePaymentProduct { get; set; }
    }
}