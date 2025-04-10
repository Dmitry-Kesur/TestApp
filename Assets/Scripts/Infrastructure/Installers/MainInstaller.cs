using Infrastructure.Data.Preloader;
using Infrastructure.Factories.Notification;
using Infrastructure.Factories.Reward;
using Infrastructure.Factories.State;
using Infrastructure.Factories.Window;
using Infrastructure.Providers.Device;
using Infrastructure.Providers.Scene;
using Infrastructure.Providers.UI;
using Infrastructure.Services;
using Infrastructure.Services.Addressable;
using Infrastructure.Services.Booster;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Currency;
using Infrastructure.Services.Hud;
using Infrastructure.Services.Items;
using Infrastructure.Services.Notification;
using Infrastructure.Services.Preloader;
using Infrastructure.Services.Reward;
using Infrastructure.Services.Sound;
using Infrastructure.Services.Window;
using Infrastructure.StateMachine;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private UIProvider _uiProvider;
        [SerializeField] private SceneProvider sceneProvider;
        [SerializeField] private PreloaderSettings _preloaderSettings;

        public override void InstallBindings()
        {
            BindSettings();
            BindProviders();
            BindFactories();
            BindServices();
        }

        private void BindFactories()
        {
            Container.Bind<IStatesFactory>().To<StatesFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<WindowFactory>().AsSingle();
            Container.Bind<NotificationsFactory>().AsSingle();
            Container.Bind<IReceiveRewardsControllersFactory>().To<ReceiveRewardsControllersFactory>().AsSingle();
        }

        private void BindSettings()
        {
            Container.BindInstance(_preloaderSettings);
        }

        private void BindProviders()
        {
            Container.Bind<UIProvider>().FromInstance(_uiProvider).AsSingle();
            Container.Bind<SceneProvider>().FromInstance(sceneProvider).AsSingle();
            Container.Bind<DeviceInfoProvider>().AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<LocalAddressableService>().AsSingle();
            Container.Bind<PrefabInstantiationService>().AsSingle();
            Container.Bind<StateMachineService>().AsSingle();
            Container.Bind<BootstrapService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemsSpawnService>().AsSingle();
            Container.Bind<IPreloaderService>().To<PreloaderService>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundService>().AsSingle();
            Container.BindInterfacesAndSelfTo<HudService>().AsSingle();
            Container.Bind<ICurrencyService>().To<CurrencyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<RewardsService>().AsSingle();
            Container.Bind<IReceiveRewardsService>().To<ReceiveRewardsService>().AsSingle();
            Container.Bind<INotificationService>().To<NotificationService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoostersService>().AsSingle();
        }
    }
}