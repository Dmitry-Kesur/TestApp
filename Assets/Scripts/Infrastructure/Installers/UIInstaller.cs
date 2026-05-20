using Infrastructure.Providers.UI;
using Infrastructure.Services.Hud;
using Infrastructure.Services.Window;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private UIProvider _uiProvider;
        
        public override void InstallBindings()
        {
            BindProviders();
            BindServices();
        }

        private void BindProviders()
        {
            Container.Bind<UIProvider>().FromInstance(_uiProvider).AsSingle();
        }

        private void BindServices()
        {
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.BindInterfacesAndSelfTo<HudService>().AsSingle();
        }
    }
}