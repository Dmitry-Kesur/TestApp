using System.Collections.Generic;

namespace Infrastructure.Services.Progress.PlayerProgressUpdaters
{
    public class PurchaseProgressUpdater : ProgressUpdater
    {
        public List<int> GetPurchasedShopProductIds() =>
            progress.PurchasedShopProductIds;

        public void SetPurchasedShopProductId(int productId)
        {
            progress.PurchasedShopProductIds.Add(productId);
        }
    }
}