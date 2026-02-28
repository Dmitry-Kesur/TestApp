using System;
using System.Threading.Tasks;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.Progress
{
    public interface ISaveLoadProgressService
    {
        TResult Read<TResult>(Func<ProgressData, TResult> read);
        void Write(Action<ProgressData> edit);
        Task OnReadyToLoadProgress(string userId);
    }
}