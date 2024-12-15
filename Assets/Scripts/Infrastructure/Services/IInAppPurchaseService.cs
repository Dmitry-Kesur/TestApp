namespace Infrastructure.Services
{
    public interface IInAppPurchaseService
    {
        void PurchaseProduct(string productId);
    }
}