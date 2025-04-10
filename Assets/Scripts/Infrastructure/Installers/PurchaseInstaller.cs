using Infrastructure.Factories.Purchase;
using Infrastructure.Providers.InAppPurchase;
using Infrastructure.Services.InAppPurchase;
using Infrastructure.Services.InGamePurchase;
using Zenject;

namespace Infrastructure.Installers
{
    public class PurchaseInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindProviders();
            BindFactories();
            BindServices();
        }

        private void BindProviders()
        {
            Container.BindInterfacesAndSelfTo<InAppPurchaseProvider>().AsSingle();
        }

        private void BindFactories()
        {
            Container.Bind<IShopProductStrategiesFactory>().To<ShopProductStrategiesFactory>().AsSingle();
        }

        private void BindServices()
        {
            BindInAppProductsService();
            BindPaymentShopService();
            BindShopService();
        }

        private void BindInAppProductsService() =>
            Container.BindInterfacesAndSelfTo<InAppPurchaseService>().AsSingle();

        private void BindPaymentShopService() =>
            Container.Bind<IPaymentShopService>().To<PaymentShopService>().AsSingle();

        private void BindShopService() =>
            Container.BindInterfacesAndSelfTo<ShopService>().AsSingle();
    }
}