using System.Collections.Generic;
using Infrastructure.Enums;
using Infrastructure.Factories;
using Zenject;

namespace Infrastructure.Services
{
    public class PurchaseProcessorsProvider : IInitializable
    {
        private readonly PurchaseProcessorsFactory _processorsFactory;
        
        private List<IPurchaseProcessor> _purchaseProcessors;

        public PurchaseProcessorsProvider(PurchaseProcessorsFactory processorsFactory)
        {
            _processorsFactory = processorsFactory;
        }

        public void Initialize()
        {
            _purchaseProcessors = _processorsFactory.CreateProcessors();
        }

        public IPurchaseProcessor GetPurchaseProcessor(InAppProductType productType) =>
            _purchaseProcessors.Find(processor => processor.ProductType == productType);
    }
}