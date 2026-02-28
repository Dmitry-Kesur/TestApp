using UnityEngine;

namespace Infrastructure.Data.Products
{
    public enum ShopProductPurchaseType
    {
        Consumable,
        NonConsumable
    }
    
    [CreateAssetMenu(fileName = "ShopProductData", menuName = "ScriptableObjects/CreateShopProductData")]
    public class ShopProductData : ScriptableObject
    {
        public int ProductId;
        public Sprite Icon;
        public ShopProductPurchaseType PurchaseType;
        public PurchaseReward PurchaseReward;
        public ResourceCost ResourceCost;
    }
}