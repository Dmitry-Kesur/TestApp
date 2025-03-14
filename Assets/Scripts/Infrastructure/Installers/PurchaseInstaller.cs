using Infrastructure.Factories;
using Infrastructure.Providers;
using Infrastructure.Services;
using Zenject;

namespace Infrastructure.Installers
{
    public class PurchaseInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactories();
            BindProviders();
            BindServices();
        }

        private void BindFactories()
        {
            Container.Bind<PurchaseProcessorsFactory>().AsSingle();
        }

        private void BindProviders()
        {
            Container.BindInterfacesAndSelfTo<InAppPurchaseProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<PurchaseProcessorsProvider>().AsSingle().NonLazy();
        }

        private void BindServices()
        {
            BindInAppProductsService();
        }

        private void BindInAppProductsService() =>
            Container.BindInterfacesAndSelfTo<InAppPurchaseService>().AsSingle();
    }
}