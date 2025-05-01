using UnityEngine;

namespace Infrastructure.Providers.InAppPurchase
{
    public class PendingPurchaseStorage
    {
        private const string KeyPrefix = "pending_purchase_";

        public void MarkAsPending(string productId)
        {
            PlayerPrefs.SetInt(KeyPrefix + productId, 1);
            PlayerPrefs.Save();
        }

        public void RemovePending(string productId)
        {
            PlayerPrefs.DeleteKey(KeyPrefix + productId);
            PlayerPrefs.Save();
        }

        public bool IsPending(string productId) =>
            PlayerPrefs.GetInt(KeyPrefix + productId, 0) == 1;
    }
}