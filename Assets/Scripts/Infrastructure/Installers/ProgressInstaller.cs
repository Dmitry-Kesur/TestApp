using Infrastructure.Factories;
using Infrastructure.Services;
using Infrastructure.Services.PlayerProgressUpdaters;
using Zenject;

namespace Infrastructure.Installers
{
    public class ProgressInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindProgressFactory();
            BindServices();
        }

        private void BindServices()
        {
            BindProgressUpdaters();
            BindSaveLoadProgressService();
            BindProgressService();
        }

        private void BindProgressFactory() =>
            Container.BindInterfacesAndSelfTo<ProgressFactory>().AsSingle();

        private void BindSaveLoadProgressService() =>
            Container.BindInterfacesAndSelfTo<SaveLoadProgressService>().AsSingle();

        private void BindProgressService() =>
            Container.BindInterfacesAndSelfTo<ProgressService>().AsSingle();

        private void BindProgressUpdaters()
        {
            Container.BindInterfacesAndSelfTo<SettingsProgressUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemsProgressUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelProgressUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResourceProgressUpdater>().AsSingle();
        }
    }
}