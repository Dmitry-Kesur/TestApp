using Infrastructure.Enums;
using UnityEngine;

namespace Infrastructure.Data.Sounds
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/CreateSoundData")]
    public class SoundData : ScriptableObject
    {
        public SoundId SoundId;
        public AudioClip SoundClip;
    }
}