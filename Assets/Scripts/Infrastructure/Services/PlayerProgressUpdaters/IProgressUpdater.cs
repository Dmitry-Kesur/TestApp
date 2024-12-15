using System;
using Infrastructure.Data.PlayerProgress;

namespace Infrastructure.Services.PlayerProgressUpdaters
{
    public interface IProgressUpdater
    {
        Action OnProgressChanged { get; set; }
        void UpdateProgress(PlayerProgress playerProgress);
        void OnLoadProgress(PlayerProgress playerProgress);
    }
}