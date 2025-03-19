using Infrastructure.Controllers.Levels;
using Infrastructure.Factories.Level;
using Infrastructure.Providers;
using Infrastructure.Providers.Level;
using Infrastructure.Services;
using Infrastructure.Services.Level;
using Zenject;

namespace Infrastructure.Installers
{
    public class LevelsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactories();
            BindServices();
        }

        private void BindFactories()
        {
            Container.Bind<ItemModelsFactory>().AsSingle();
            Container.Bind<LevelViewsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemViewsFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelsFactory>().AsSingle();
        }

        private void BindServices()
        {
            BindLevelsStaticDataProvider();
            BindLevelsService();
            BindProgressController();
            BindItemsController();
        }

        private void BindLevelsStaticDataProvider() =>
            Container.BindInterfacesAndSelfTo<LevelsStaticDataProvider>().AsSingle();

        private void BindLevelsService() =>
            Container.Bind<ILevelsService>().To<LevelService>().AsSingle();

        private void BindProgressController() =>
            Container.Bind<ProgressController>().AsSingle();

        private void BindItemsController() =>
            Container.Bind<ItemsSpawnController>().AsSingle();
    }
}