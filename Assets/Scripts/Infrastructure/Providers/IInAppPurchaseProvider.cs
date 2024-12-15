using System;
using Infrastructure.Data.Products;

namespace Infrastructure.Providers
{
    public interface IInAppPurchaseProvider
    {
        void Initialize();
        void Purchase(string productId);
        Action<InAppProductData> OnProcessPurchaseAction { get; set; }
        Action OnInitializedAction { get; set; }
    }
}