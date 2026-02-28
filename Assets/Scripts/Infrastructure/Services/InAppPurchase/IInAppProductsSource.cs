using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data.Products;

namespace Infrastructure.Services.InAppPurchase
{
    public interface IInAppProductsSource
    {
        Task<List<InAppProductData>> GetProducts();
    }
}