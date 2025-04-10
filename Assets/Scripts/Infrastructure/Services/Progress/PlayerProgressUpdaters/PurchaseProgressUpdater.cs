using System.Collections.Generic;

namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public class PurchaseProgressUpdater : ProgressUpdater
    {
        public bool HasPendingInAppPurchaseProduct(string productId)
        {
            var pendingProducts = progress.PendingInAppPurchaseProducts;
            return pendingProducts.Contains(productId);
        }

        public List<int> GetPurchasedShopProductIds() =>
            progress.PurchasedShopProductIds;

        public void SetPendingInAppPurchaseProduct(string productId)
        {
            progress.PendingInAppPurchaseProducts.Add(productId);
        }

        public void SetPurchasedShopProductId(int productId)
        {
            progress.PurchasedShopProductIds.Add(productId);
        }

        public void RemovePendingInAppPurchaseProduct(string productId)
        {
            if (HasPendingInAppPurchaseProduct(productId))
                progress.PendingInAppPurchaseProducts.Remove(productId);
        }
    }
}