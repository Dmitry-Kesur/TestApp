using System;
using Infrastructure.Enums;

namespace Infrastructure.Services.Sound
{
    public interface ISoundService
    {
        bool MuteSounds { get; }
        void PlaySound(SoundId soundId);
        void ChangeMuteSounds(bool muteSounds);
        public Action<bool> OnChangeMuteSoundsAction { get; set; }
    }
}