using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Serialization;

namespace Infrastructure.Data.Products
{
    [CreateAssetMenu(fileName = "InAppProductData", menuName = "ScriptableObjects/CreateInAppProductData")]
    public class InAppProductData : ScriptableObject
    {
        public string productId;
        public Sprite productIcon;
        public ProductType productLifetimeType;
        public PurchaseReward purchaseReward;
    }
}