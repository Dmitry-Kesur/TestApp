using System.Collections.Generic;
using Zenject;

namespace Infrastructure.Services
{
    public class PurchaseProcessorsResolver
    {
        private readonly DiContainer _diContainer;

        public PurchaseProcessorsResolver(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public List<IPurchaseProcessor> GetProcessors()
        {
            var processors = _diContainer.ResolveAll<IPurchaseProcessor>();
            return processors;
        }
    }
}