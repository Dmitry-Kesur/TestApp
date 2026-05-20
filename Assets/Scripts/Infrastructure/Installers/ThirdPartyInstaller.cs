using Infrastructure.Services;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Log;
using Infrastructure.Services.Reward;
using Zenject;

namespace Infrastructure.Installers
{
    public class ThirdPartyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindServices();
        }

        private void BindServices()
        {
            BindUnityCoreInitializer();
            BindFirebaseCoreInitializer();
            BindAnalyticsService();
            BindExceptionLoggerService();
            BindAdsService();
            BindAuthenticationService();
            BindDailyAdRewardService();
        }

        private void BindUnityCoreInitializer()
        {
            Container.Bind<ICoreThirdPartyInitializable>()
                .To<UnityCoreInitializer>()
                .AsSingle();
        }

        private void BindFirebaseCoreInitializer()
        {
            Container.Bind<ICoreThirdPartyInitializable>()
                .To<FirebaseCoreInitializer>()
                .AsSingle();
        }

        private void BindAnalyticsService() =>
            Container.BindInterfacesAndSelfTo<AnalyticsService>().AsSingle();

        private void BindExceptionLoggerService()
        {
#if !UNITY_EDITOR
            Container.BindInterfacesAndSelfTo<CrashlyticsService>().AsSingle();
#else
            Container.BindInterfacesAndSelfTo<EditorExceptionLoggerService>().AsSingle();
#endif
        }

        private void BindAdsService() =>
            Container.BindInterfacesAndSelfTo<AdsService>().AsSingle();

        private void BindAuthenticationService()
        {
#if UNITY_EDITOR
            Container.BindInterfacesAndSelfTo<EditorAuthenticationService>().AsSingle();
#else
                Container.BindInterfacesAndSelfTo<GoogleAuthenticationService>().AsSingle();
#endif
        }

        private void BindDailyAdRewardService() =>
            Container.BindInterfacesAndSelfTo<DailyAdsService>().AsSingle();
    }
}