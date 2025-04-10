using System;
using Infrastructure.Data.Products;
using Infrastructure.Models.UI.Items;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.Products.InGame
{
    public class ProductModel : IDrawableModel, IProductModel
    {
        private readonly ProductData _productData;
        private readonly IPurchaseProductTarget _purchaseProductTarget;
        
        public Action<ProductModel> OnPurchaseProductAction;

        public ProductModel(ProductData productData, IPurchaseProductTarget purchaseProductTarget)
        {
            _productData = productData;
            _purchaseProductTarget = purchaseProductTarget;
        }

        public int Id => 
            _productData.ProductId;

        public Sprite ProductIcon =>
            IconSprite;

        public int Price => 
            _productData.Price;

        public Sprite IconSprite =>
            _productData.Icon;

        public bool Purchased { get; set; }
        
        public IPurchaseProductTarget GetPurchaseTarget() => 
            _purchaseProductTarget;

        public Action UpdateProductAction { get; set; }

        public void Purchase() =>
            OnPurchaseProductAction?.Invoke(this);
    }
}