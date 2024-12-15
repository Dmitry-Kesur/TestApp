using Infrastructure.Enums;

namespace Infrastructure.Services
{
    public interface ISoundService
    {
        bool MuteSounds { get; }
        void PlaySound(SoundId soundId);
        void ChangeMuteSounds(bool muteSounds);
    }
}