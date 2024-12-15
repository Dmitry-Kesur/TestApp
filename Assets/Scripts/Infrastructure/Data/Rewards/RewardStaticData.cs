using Infrastructure.Enums;
using UnityEngine;

namespace Infrastructure.Data.Rewards
{
    [CreateAssetMenu(fileName = "StaticRewardData", menuName = "ScriptableObjects/CreateStaticRewardData")]
    public class RewardStaticData : ScriptableObject
    {
        public int Id;
        public RewardType Type;
        public Sprite Sprite;
    }
}