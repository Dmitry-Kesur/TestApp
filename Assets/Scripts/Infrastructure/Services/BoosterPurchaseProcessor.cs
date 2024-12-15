using Infrastructure.Enums;

namespace Infrastructure.Services
{
    public class BoosterPurchaseProcessor : IPurchaseProcessor
    {
        private readonly IBoostersService _boostersService;

        public BoosterPurchaseProcessor(IBoostersService boostersService)
        {
            _boostersService = boostersService;
        }
        
        public void ProcessPurchase(string productId)
        {
            _boostersService.OnCompletePurchaseBooster(productId);
        }

        public InAppProductType ProductType =>
            InAppProductType.Booster;
    }
}