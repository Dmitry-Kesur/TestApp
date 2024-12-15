using System;
using Infrastructure.Models.GameEntities.Products;
using Infrastructure.Models.GameEntities.Products.InGame;

namespace Infrastructure.Services
{
    public interface IPaymentProductService
    {
        void PaymentProduct(IProductModel product);
        Action<IProductModel> OnCompletePaymentProduct { get; set; }
    }
}