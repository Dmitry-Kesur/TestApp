using Infrastructure.Factories.Shop;
using Infrastructure.Providers.InAppPurchase;
using Infrastructure.Services.InAppPurchase;
using Infrastructure.Services.Shop;
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
            Container.Bind<ShopProductFactory>().AsSingle();
        }

        private void BindServices()
        {
            BindInAppProductsSource();
            BindCrossPlatformPurchaseValidator();
            BindInAppProductsService();
            BindShopProductRewardResolver();
            BindPaymentShopService();
            BindShopService();
        }

        private void BindInAppProductsSource() =>
            Container.Bind<IInAppProductsSource>().To<InAppProductsSource>().AsSingle();

        private void BindCrossPlatformPurchaseValidator() =>
            Container.Bind<IPurchaseValidator>().To<CrossPlatformPurchaseValidator>().AsSingle();

        private void BindInAppProductsService() =>
            Container.BindInterfacesAndSelfTo<InAppPurchaseService>().AsSingle();

        private void BindShopProductRewardResolver() =>
            Container.Bind<ShopProductRewardResolver>().AsSingle();
        
        private void BindPaymentShopService() =>
            Container.Bind<IPaymentShopService>().To<PaymentShopService>().AsSingle();

        private void BindShopService() =>
            Container.BindInterfacesAndSelfTo<ShopService>().AsSingle();
    }
}