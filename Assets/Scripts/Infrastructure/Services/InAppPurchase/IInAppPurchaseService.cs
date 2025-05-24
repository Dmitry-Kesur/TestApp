using System;
using System.Threading.Tasks;

namespace Infrastructure.Services.InAppPurchase
{
    public interface IInAppPurchaseService
    {
        Action<string> OnCompletePurchase { get; set; }
        Task PurchaseProduct(string productId);
    }
}