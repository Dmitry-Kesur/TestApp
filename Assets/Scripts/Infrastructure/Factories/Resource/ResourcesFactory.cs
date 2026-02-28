using Infrastructure.Data;
using Infrastructure.Models.GameEntities.Resources;
using Zenject;

namespace Infrastructure.Factories
{
    public class ResourcesFactory
    {
        private readonly DiContainer _diContainer;

        public ResourcesFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public ResourceModel Create(ResourceData resourceData)
        {
            var model = _diContainer.Instantiate<ResourceModel>();
            model.SetData(resourceData);
            return model;
        }
    }
}