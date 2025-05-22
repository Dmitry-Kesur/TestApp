using System.Collections.Generic;

namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public class PurchaseProgressUpdater : ProgressUpdater
    {
        public List<int> GetPurchasedShopProductIds() =>
            progress.PurchasedShopProductIds;

        public void SetPurchasedShopProductId(int productId) =>
            progress.PurchasedShopProductIds.Add(productId);

        public void MarkProductAsPending(string productId) =>
            progress.PendingInAppProducts.Add(productId);

        public bool CheckPending(string productId) =>
            progress.PendingInAppProducts.Contains(productId);

        public void RemovePendingProduct(string productId) =>
            progress.PendingInAppProducts.Remove(productId);
    }
}