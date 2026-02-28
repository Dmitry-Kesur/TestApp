using Infrastructure.Data;
using Infrastructure.Data.Products;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.Shop
{
    public interface IShopProductModel
    {
        ResourceCost Cost { get; }
        int Id { get; }
        bool Purchased { get; set; }
        Sprite ProductIcon { get; }
        PurchaseReward PurchaseReward { get; }
        ShopProductPurchaseType PurchaseType { get; }
    }
}