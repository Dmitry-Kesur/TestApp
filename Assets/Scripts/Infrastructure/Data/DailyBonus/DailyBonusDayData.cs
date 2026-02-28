using UnityEngine;

namespace Infrastructure.Data.DailyBonus
{
    [CreateAssetMenu(fileName = "DailyBonusDayData", menuName = "ScriptableObjects/CreateDailyBonusDayData")]
    public class DailyBonusDayData : ScriptableObject
    {
        public int Day;
        public int RewardResourceId;
        public int RewardAmount;
    }
}