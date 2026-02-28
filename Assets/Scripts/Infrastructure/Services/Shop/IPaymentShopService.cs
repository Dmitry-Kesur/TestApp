using System;
using Infrastructure.Models.GameEntities.Shop;

namespace Infrastructure.Services.Shop
{
    public interface IPaymentShopService
    {
        void PaymentProduct(IShopProductModel iShopProduct);
        Action<IShopProductModel> OnCompletePaymentProduct { get; set; }
    }
}