using System.Collections.Generic;
using Infrastructure.Data.Products;
using Infrastructure.Models.GameEntities.Products;
using Infrastructure.Models.GameEntities.Products.InGame;

namespace Infrastructure.Strategy
{
    public interface IProductStrategy
    {
        List<ProductModel> CreateProducts(List<ProductData> products);
    }
}