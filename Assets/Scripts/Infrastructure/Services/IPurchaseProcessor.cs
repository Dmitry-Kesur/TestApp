using Infrastructure.Enums;

namespace Infrastructure.Services
{
    public interface IPurchaseProcessor
    {
        void ProcessPurchase(string productId);
        InAppProductType ProductType { get; }
    }
}