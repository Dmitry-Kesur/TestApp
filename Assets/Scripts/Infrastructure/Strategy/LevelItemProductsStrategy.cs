using System.Collections.Generic;
using Infrastructure.Data.Products;
using Infrastructure.Enums;
using Infrastructure.Models.GameEntities.Products;
using Infrastructure.Models.GameEntities.Products.InGame;
using Infrastructure.Services;

namespace Infrastructure.Strategy
{
    public class LevelItemProductsStrategy : IProductStrategy
    {
        private readonly IItemsService _itemsService;

        public LevelItemProductsStrategy(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }
        
        public List<ProductModel> CreateProducts(List<ProductData> products)
        {
            List<ProductModel> productModels = new();
            
            var levelItemProducts =
                products.FindAll(productData => productData.ProductCategory == ProductCategory.LevelItem);

            foreach (var levelItemProduct in levelItemProducts)
            {
                var purchaseProductTarget = _itemsService.GetItemById(levelItemProduct.ProductId) as IPurchaseProductTarget;
                var productModel = new ProductModel(levelItemProduct, purchaseProductTarget);
                productModels.Add(productModel);
            }

            return productModels;
        }
    }
}