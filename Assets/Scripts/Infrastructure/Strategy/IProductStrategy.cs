using System.Collections.Generic;
using Infrastructure.Data.Products;
using Infrastructure.Models.GameEntities.Shop;

namespace Infrastructure.Strategy
{
    public interface IProductStrategy
    {
        List<ProductModel> CreateProducts(List<ProductData> products);
    }
}