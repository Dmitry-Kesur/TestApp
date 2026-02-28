using Infrastructure.Factories;
using Infrastructure.Factories.Booster;
using Infrastructure.Services;
using Infrastructure.Services.Resource;
using Zenject;

namespace Infrastructure.Installers
{
    public class ResourceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactories();
            BindServices();
        }

        private void BindFactories()
        {
            Container.Bind<ResourcesFactory>().AsSingle();
            Container.Bind<BoosterFactory>().AsSingle();
        }

        private void BindServices()
        {
            BindResourcesService();
        }

        private void BindResourcesService() =>
            Container.BindInterfacesAndSelfTo<ResourcesService>().AsSingle();
    }
}