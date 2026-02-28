using UnityEngine;

namespace Infrastructure.Data.Boosters
{
    [CreateAssetMenu(fileName = "BoosterData", menuName = "ScriptableObjects/CreateBoosterData")]
    public class BoosterData : ScriptableObject
    {
        public int Id;
        public int BoostValue;
        public int DurationSeconds;
        public Sprite IconSprite;
        public int RequiredResourceId;
    }
}