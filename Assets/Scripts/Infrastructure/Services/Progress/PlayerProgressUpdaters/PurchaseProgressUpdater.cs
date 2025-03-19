using System.Collections.Generic;

namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public class PurchaseProgressUpdater : ProgressUpdater
    {
        public bool HasPendingPurchaseProduct(string productId)
        {
            var pendingProducts = progress.PendingInAppPurchaseProducts;
            return pendingProducts.Contains(productId);
        }

        public List<int> GetPurchasedInGameProductIds() =>
            progress.PurchasedInGameProductIds;

        public void SetPendingPurchaseInAppProduct(string productId)
        {
            progress.PendingInAppPurchaseProducts.Add(productId);
        }

        public void SetPurchasedInGameProductId(int productId)
        {
            progress.PurchasedInGameProductIds.Add(productId);
        }

        public void RemovePendingPurchaseProduct(string productId)
        {
            if (HasPendingPurchaseProduct(productId))
                progress.PendingInAppPurchaseProducts.Remove(productId);
        }
    }
}