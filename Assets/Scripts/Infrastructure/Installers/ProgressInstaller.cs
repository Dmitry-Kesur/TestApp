using Infrastructure.Factories.Progress;
using Infrastructure.Services.Progress;
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
            BindFirebaseProgressRepository();
            BindSaveLoadProgressService();
        }

        private void BindFirebaseProgressRepository() =>
            Container.Bind<IProgressRepository>().To<FirebaseProgressRepository>().AsSingle();

        private void BindProgressFactory() =>
            Container.BindInterfacesAndSelfTo<ProgressFactory>().AsSingle();

        private void BindSaveLoadProgressService() =>
            Container.BindInterfacesAndSelfTo<SaveLoadProgressService>().AsSingle();
    }
}