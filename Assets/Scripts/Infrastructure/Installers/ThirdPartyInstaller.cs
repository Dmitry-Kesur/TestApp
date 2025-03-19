using Infrastructure.Services;
using Infrastructure.Services.Ads;
using Infrastructure.Services.Analytics;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.Log;
using Infrastructure.Services.RemoteConfig;
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
            BindAnalyticsService();
            BindExceptionLoggerService();
            BindRemoteConfigService();
            BindAdsService();
            BindAuthenticationService();
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

        private void BindRemoteConfigService() =>
            Container.BindInterfacesAndSelfTo<RemoteConfigService>().AsSingle();

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
    }
}