using UnityEngine;

namespace Infrastructure.Data
{
    [CreateAssetMenu(fileName = "DailyAdsRewardData", menuName = "ScriptableObjects/DailyAdsRewardData")]
    public class DailyAdsRewardData : ScriptableObject
    {
        public int ResourceId;
        public int Amount;
    }
}