using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data.Preloader;
using Infrastructure.Services.Log;
using Zenject;

namespace Infrastructure.Services.Preloader
{
    public class PreloaderService : IPreloaderService
    {
        private Action<float, string> _updateLoadingProgress;
        
        private readonly DiContainer _diContainer;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly PreloaderSettings _preloaderSettings;

        public PreloaderService(DiContainer diContainer, IExceptionLoggerService exceptionLoggerService)
        {
            _diContainer = diContainer;
            _exceptionLoggerService = exceptionLoggerService;
            _preloaderSettings = _diContainer.Resolve<PreloaderSettings>();
        }

        public Action<float, string> UpdateLoadingProgressAction
        {
            set => _updateLoadingProgress = value;
        }

        public async Task Load()
        {
            var loadableServices = _diContainer.ResolveAll<ILoadableService>();
            SortLoadableTargets(loadableServices);
            foreach (var loadableService in loadableServices)
            {
                var data = _preloaderSettings.GetData(loadableService.LoadingStage);

                try
                {
                    await loadableService.Load();
                }
                catch (Exception e)
                {
                    _exceptionLoggerService.LogException(e);
                    Console.WriteLine(e);
                    throw;
                }
                
                _updateLoadingProgress?.Invoke(data.ProgressValue, data.StageText);
            }
        }
        
        private void SortLoadableTargets(List<ILoadableService> loadableServices)
        {
            loadableServices.Sort((targetOne, targetTwo) =>
            {
                var dataOne = _preloaderSettings.GetData(targetOne.LoadingStage);
                var dataTwo = _preloaderSettings.GetData(targetTwo.LoadingStage);

                return dataOne.SortValue.CompareTo(dataTwo.SortValue);
            });
        }
    }
}