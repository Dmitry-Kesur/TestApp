using System;
using System.Threading.Tasks;
using Infrastructure.Data.Products;

namespace Infrastructure.Services.InAppPurchase
{
    public interface IInAppPurchaseService
    {
        Action<PurchaseReward> OnCompletePurchaseAction { get; set; }
        Task PurchaseProduct(string productId);
    }
}