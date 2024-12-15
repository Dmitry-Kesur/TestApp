﻿using Infrastructure.Controllers.Levels;
using Infrastructure.Data.Preloader;
using Infrastructure.Factories;
using Infrastructure.Providers;
using Infrastructure.Services;
using Infrastructure.StateMachine;
using Infrastructure.Utils;
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
            Container.Bind<INotificationsFactory>().To<NotificationsFactory>().AsSingle();
            Container.Bind<IReceiveRewardsControllersFactory>().To<ReceiveRewardsControllersFactory>().AsSingle();
            Container.Bind<IProductStrategiesFactory>().To<ProductStrategiesFactory>().AsSingle();
        }

        private void BindSettings()
        {
            Container.BindInstance(_preloaderSettings);
        }

        private void BindProviders()
        {
            Container.Bind<UIProvider>().FromInstance(_uiProvider).AsSingle();
            Container.Bind<SceneProvider>().FromInstance(sceneProvider).AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<LocalAddressableService>().AsSingle();
            Container.Bind<IPrefabInstantiationService>().To<PrefabInstantiationService>().AsSingle();
            Container.Bind<StateMachineService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemsService>().AsSingle();
            Container.BindInterfacesAndSelfTo<InGamePurchaseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemsSpawnService>().AsSingle();
            Container.Bind<IPreloaderService>().To<PreloaderService>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundService>().AsSingle();
            Container.BindInterfacesAndSelfTo<HudService>().AsSingle();
            Container.Bind<ICurrencyService>().To<PlayerCurrencyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<RewardsService>().AsSingle();
            Container.Bind<IReceiveRewardsService>().To<ReceiveRewardsService>().AsSingle();
            Container.Bind<INotificationService>().To<NotificationService>().AsSingle();
            Container.Bind<IPaymentProductService>().To<PaymentProductService>().AsSingle();
            Container.BindInterfacesAndSelfTo<BoostersService>().AsSingle();
        }
    }
}