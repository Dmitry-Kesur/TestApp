using UnityEngine;

namespace Infrastructure.Data.LuckySpin
{
    [CreateAssetMenu(fileName = "WheelSegmentData", menuName = "ScriptableObjects/CreateWheelSegmentData")]
    public class WheelSegmentData : ScriptableObject
    {
        public int Index;
        public int Weight;
        public int RewardId;
        public int RewardAmount;
    }
}