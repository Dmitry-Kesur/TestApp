using Infrastructure.Controllers.Levels;
using Infrastructure.Factories.Level;
using Infrastructure.Providers.Level;
using Infrastructure.Services.Level;
using Zenject;

namespace Infrastructure.Installers
{
    public class LevelsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactories();
            BindProviders();
            BindServices();
        }

        private void BindFactories()
        {
            Container.BindInterfacesAndSelfTo<ItemsStrategyFactory>().AsSingle();
            Container.Bind<ItemModelsFactory>().AsSingle();
            Container.Bind<LevelViewsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemViewsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelModelsFactory>().AsSingle();
        }

        private void BindProviders()
        {
            BindLevelsStaticDataProvider();
        }

        private void BindServices()
        {
            BindLevelsService();
            BindProgressController();
            BindItemsInteractionController();
            BindItemsSpawnController();
        }

        private void BindLevelsStaticDataProvider() =>
            Container.BindInterfacesAndSelfTo<LevelsStaticDataProvider>().AsSingle();

        private void BindLevelsService() =>
            Container.Bind<ILevelsService>().To<LevelService>().AsSingle();

        private void BindProgressController() =>
            Container.Bind<LevelProgressController>().AsSingle();

        private void BindItemsInteractionController() =>
            Container.Bind<ItemsInteractionController>().AsSingle();

        private void BindItemsSpawnController() =>
            Container.Bind<ItemsSpawnController>().AsSingle();
    }
}