using Infrastructure.Data.Products;
using Infrastructure.Models.GameEntities.Shop;

namespace Infrastructure.Factories.Shop
{
    public class ShopProductFactory
    {
        public ShopProductModel CreateProductModel(ShopProductData productData)
        {
            var productModel = new ShopProductModel(productData);
            return productModel;
        }
    }
}