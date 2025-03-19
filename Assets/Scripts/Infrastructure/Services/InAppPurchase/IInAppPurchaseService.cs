using System;

namespace Infrastructure.Services.InAppPurchase
{
    public interface IInAppPurchaseService
    {
        Action<string> OnCompletePurchase { get; set; }
        void PurchaseProduct(string productId);
    }
}