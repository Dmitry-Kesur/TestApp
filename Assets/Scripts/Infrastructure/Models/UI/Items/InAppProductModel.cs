using System;
using Infrastructure.Data.Products;
using UnityEngine;

namespace Infrastructure.Models.UI.Items
{
    public class InAppProductModel : IDrawableModel
    {
        public Action<string> OnPurchaseAction;
        
        private readonly InAppProductData _productData;

        public InAppProductModel(InAppProductData productData)
        {
            _productData = productData;
        }

        public int PurchaseRewardAmount => PurchaseReward.Amount;
        
        public string Price { get; set; }

        public string ProductId => _productData.productId;

        public Sprite IconSprite => _productData.productIcon;

        public PurchaseReward PurchaseReward => _productData.purchaseReward;

        public void Purchase() =>
            OnPurchaseAction?.Invoke(ProductId);
    }
}