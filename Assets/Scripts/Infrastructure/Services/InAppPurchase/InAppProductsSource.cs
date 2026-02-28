using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Products;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Log;

namespace Infrastructure.Services.InAppPurchase
{
    public class InAppProductsSource : IInAppProductsSource
    {
        private readonly LocalAddressableService _localAddressableService;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        public InAppProductsSource(LocalAddressableService localAddressableService,
            IExceptionLoggerService exceptionLoggerService)
        {
            _localAddressableService = localAddressableService;
            _exceptionLoggerService = exceptionLoggerService;
        }

        public async Task<List<InAppProductData>> GetProducts()
        {
            var products = await LoadAllProducts();
            return products;
        }

        private async Task<List<InAppProductData>> LoadAllProducts()
        {
            var products = await _localAddressableService.LoadScriptableCollectionFromGroupAsync<InAppProductData>(
                AddressableGroupNames
                    .InAppProductsGroup);
            return products;
        }
    }
}