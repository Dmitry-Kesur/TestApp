using UnityEngine;

namespace Infrastructure.Data.Boosters
{
    [CreateAssetMenu(fileName = "BoosterData", menuName = "ScriptableObjects/CreateBoosterData")]
    public class BoosterData : ScriptableObject
    {
        public int Id;
        public int BoostValue;
        public Sprite IconSprite;
        public string ProductId;
    }
}