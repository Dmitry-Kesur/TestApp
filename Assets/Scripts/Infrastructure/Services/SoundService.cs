using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Constants;
using Infrastructure.Data.Preloader;
using Infrastructure.Data.Sounds;
using Infrastructure.Enums;
using Infrastructure.Providers;
using Infrastructure.Services.PlayerProgressUpdaters;
using UnityEngine;

namespace Infrastructure.Services
{
    public class SoundService : ISoundService, ILoadableService
    {
        private readonly Dictionary<SoundId, AudioClip> _soundClips = new();
        private readonly AudioSource _audioSource;
        private readonly LocalAddressableService _localAddressableService;
        private readonly SettingsProgressUpdater _settingsProgressUpdater;
        private readonly IExceptionLoggerService _exceptionLoggerService;

        public SoundService(SceneProvider sceneProvider, LocalAddressableService localAddressableService, SettingsProgressUpdater settingsProgressUpdater, IExceptionLoggerService exceptionLoggerService)
        {
            _audioSource = sceneProvider.AudioSource;
            _localAddressableService = localAddressableService;
            _settingsProgressUpdater = settingsProgressUpdater;
            _exceptionLoggerService = exceptionLoggerService;
        }

        public async Task Load()
        {
            var soundsData =
                await _localAddressableService.LoadScriptableCollectionFromGroupAsync<SoundData>(AddressableGroupNames
                    .SoundsGroup);
            
            CreateSounds(soundsData);
            Update();
        }

        public bool MuteSounds => _audioSource.mute;

        public void ChangeMuteSounds(bool muteSounds)
        {
            _audioSource.mute = muteSounds;
            _settingsProgressUpdater.ChangeMuteSounds(muteSounds);
        }

        public void PlaySound(SoundId soundId)
        {
            var soundClip = _soundClips[soundId];
            _audioSource.PlayOneShot(soundClip);
        }

        public LoadingStage LoadingStage => LoadingStage.LoadingSounds;
        
        private void CreateSounds(List<SoundData> soundClips)
        {
            foreach (var soundClip in soundClips)
            {
                _soundClips.Add(soundClip.SoundId, soundClip.SoundClip);
            }
        }

        private void Update()
        {
            _audioSource.mute = _settingsProgressUpdater.MuteSounds;
        }
    }
}