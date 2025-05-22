using System;
using Infrastructure.Models.GameEntities.Shop;

namespace Infrastructure.Services.Shop
{
    public interface IPaymentShopService
    {
        void PaymentProduct(IProductModel product);
        Action<IProductModel> OnCompletePaymentProduct { get; set; }
    }
}