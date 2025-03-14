using System.Collections.Generic;
using Infrastructure.Services;
using Zenject;

namespace Infrastructure.Factories
{
    public class PurchaseProcessorsFactory
    {
        private readonly DiContainer _diContainer;

        public PurchaseProcessorsFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public List<IPurchaseProcessor> CreateProcessors()
        {
            List<IPurchaseProcessor> purchaseProcessors = new List<IPurchaseProcessor>
            {
                _diContainer.Instantiate<BoosterPurchaseProcessor>()
            };

            return purchaseProcessors;
        }
    }
}