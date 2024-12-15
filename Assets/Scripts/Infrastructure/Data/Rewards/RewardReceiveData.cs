using UnityEngine;

namespace Infrastructure.Data.Rewards
{
    [CreateAssetMenu(fileName = "RewardReceiveData", menuName = "ScriptableObjects/CreateRewardReceiveData")]
    public class RewardReceiveData : ScriptableObject
    {
        public int Id;
        public int Amount;
    }
}