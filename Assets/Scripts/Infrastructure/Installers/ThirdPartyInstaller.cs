using Infrastructure.Factories;
using Infrastructure.Providers;
using Infrastructure.Services;
using Zenject;

namespace Infrastructure.Installers
{
    public class ThirdPartyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFirebaseInitializer();
            BindProviders();
            BindServices();
            BindPurchaseProcessors();
        }

        private void BindFirebaseInitializer() =>
            Container.BindInterfacesAndSelfTo<FirebaseInitializer>().AsSingle();

        private void BindProviders()
        {
            Container.BindInterfacesAndSelfTo<InAppPurchaseProvider>().AsSingle();
        }

        private void BindServices()
        {
            BindAnalyticsService();
            BindCrashlyticsService();
            BindRemoteConfigService();
            BindAdsService();
            BindInAppProductsService();
            BindPurchaseProcessorsResolver();
            BindGoogleAuthenticationService();
        }

        private void BindPurchaseProcessors()
        {
            Container.BindInterfacesAndSelfTo<BoosterPurchaseProcessor>().AsSingle();
        }

        private void BindAnalyticsService() =>
            Container.BindInterfacesAndSelfTo<AnalyticsService>().AsSingle();

        private void BindCrashlyticsService() =>
            Container.BindInterfacesAndSelfTo<CrashlyticsService>().AsSingle();

        private void BindRemoteConfigService() =>
            Container.BindInterfacesAndSelfTo<RemoteConfigService>().AsSingle();

        private void BindAdsService() =>
            Container.BindInterfacesAndSelfTo<AdsService>().AsSingle();

        private void BindInAppProductsService() =>
            Container.BindInterfacesAndSelfTo<InAppPurchaseService>().AsSingle();

        private void BindPurchaseProcessorsResolver() =>
            Container.Bind<PurchaseProcessorsResolver>().AsSingle();

        private void BindGoogleAuthenticationService() =>
            Container.BindInterfacesAndSelfTo<GoogleAuthenticationService>().AsSingle();
    }
}