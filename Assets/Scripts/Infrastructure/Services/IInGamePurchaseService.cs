using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Products.InGame;

namespace Infrastructure.Services
{
    public interface IInGamePurchaseService
    {
        List<ProductModel> GetProducts();
    }
}