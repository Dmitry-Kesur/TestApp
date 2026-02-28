using System;
using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;
using Infrastructure.Factories.Progress;
using Infrastructure.Services.Application;
using Infrastructure.Services.Bootstrap;
using Infrastructure.Services.Log;

namespace Infrastructure.Services.Progress
{
    public class SaveLoadProgressService : ISaveLoadProgressService, IThirdPartyInitializable
    {
        private readonly IProgressFactory _factory;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly IProgressRepository _progressRepository;

        private string _userId;

        private ProgressData _progressData;

        public SaveLoadProgressService(IProgressFactory factory, IExceptionLoggerService exceptionLoggerService,
            ApplicationLifecycleWatcher lifecycleWatcher, IProgressRepository progressRepository)
        {
            _factory = factory;
            _exceptionLoggerService = exceptionLoggerService;
            _progressRepository = progressRepository;
            
            lifecycleWatcher.OnFocusOutAction = SaveProgress;
            lifecycleWatcher.OnQuitAction = SaveProgress;
        }

        public void Initialize()
        {
            _progressRepository.Initialize();
        }

        public TResult Read<TResult>(Func<ProgressData, TResult> read) =>
            read(_progressData);

        public void Write(Action<ProgressData> edit) =>
            edit(_progressData);

        public async Task OnReadyToLoadProgress(string userId)
        {
            _userId = userId;
            _progressData = await LoadProgress() ?? _factory.CreateProgress(_userId);
        }

        private async Task<ProgressData> LoadProgress()
        {
            try
            {
                return await _progressRepository.Load(_userId);
            }
            catch (Exception e)
            {
                _exceptionLoggerService.LogException(e);
                throw;
            }
        }

        private async void SaveProgress()
        {
            try
            {
                await _progressRepository.Save(_userId, _progressData);
            }
            catch (Exception e)
            {
                _exceptionLoggerService.LogException(e);
            }
        }
    }
}