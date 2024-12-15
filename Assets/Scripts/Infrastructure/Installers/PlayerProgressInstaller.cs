using Infrastructure.Services;
using Infrastructure.Services.PlayerProgressUpdaters;
using Zenject;

namespace Infrastructure.Installers
{
    public class PlayerProgressInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindProgressUpdaters();
            BindSaveLoadProgressService();
            BindPlayerProgressService();
        }

        private void BindSaveLoadProgressService() =>
            Container.BindInterfacesAndSelfTo<SaveLoadPlayerProgressService>().AsSingle();

        private void BindPlayerProgressService() =>
            Container.BindInterfacesAndSelfTo<PlayerProgressService>().AsSingle();

        private void BindProgressUpdaters()
        {
            Container.BindInterfacesAndSelfTo<SettingsProgressUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<ItemsProgressUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<LevelProgressUpdater>().AsSingle();
            Container.BindInterfacesAndSelfTo<ResourceProgressUpdater>().AsSingle();
        }
    }
}