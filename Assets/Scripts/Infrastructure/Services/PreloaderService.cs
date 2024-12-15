﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Data.Preloader;
using Zenject;

namespace Infrastructure.Services
{
    public class PreloaderService : IPreloaderService
    {
        private Action<float, string> _updateLoadingProgress;
        private readonly DiContainer _diContainer;
        private readonly PreloaderSettings _preloaderSettings;

        public PreloaderService(DiContainer diContainer)
        {
            _diContainer = diContainer;
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

                await loadableService.Load();
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