using System;
using Infrastructure.Data;
using Infrastructure.Data.Products;
using Infrastructure.Models.UI.Items;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.Shop
{
    public class ShopProductModel : IDrawableModel, IShopProductModel
    {
        private readonly ShopProductData _shopProductData;
        
        public Action<ShopProductModel> OnPurchaseProductAction;

        public ShopProductModel(ShopProductData shopProductData)
        {
            _shopProductData = shopProductData;
        }

        public int Id => 
            _shopProductData.ProductId;

        public int CostAmount => Cost.Amount;

        public Sprite ProductIcon =>
            IconSprite;
        
        public ResourceCost Cost => _shopProductData.ResourceCost;

        public PurchaseReward PurchaseReward => _shopProductData.PurchaseReward;

        public ShopProductPurchaseType PurchaseType => _shopProductData.PurchaseType;
        
        public Sprite IconSprite =>
            _shopProductData.Icon;

        public bool Purchased { get; set; }

        public void Purchase() =>
            OnPurchaseProductAction?.Invoke(this);
    }
}