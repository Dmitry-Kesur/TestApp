using Infrastructure.Providers;
using Infrastructure.Providers.InAppPurchase;
using Infrastructure.Services;
using Infrastructure.Services.InAppPurchase;
using Zenject;

namespace Infrastructure.Installers
{
    public class PurchaseInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindProviders();
            BindServices();
        }

        private void BindProviders()
        {
            Container.BindInterfacesAndSelfTo<InAppPurchaseProvider>().AsSingle();
        }

        private void BindServices()
        {
            BindInAppProductsService();
        }

        private void BindInAppProductsService() =>
            Container.BindInterfacesAndSelfTo<InAppPurchaseService>().AsSingle();
    }
}