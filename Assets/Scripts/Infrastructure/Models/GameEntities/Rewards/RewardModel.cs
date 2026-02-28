using Infrastructure.Data.Rewards;
using Infrastructure.Models.UI.Items;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.Rewards
{
    public class RewardModel : IDrawableModel
    {
        private readonly int _amount;
        
        private readonly RewardStaticData _rewardStaticData;

        public RewardModel(RewardStaticData rewardStaticData, int rewardAmount)
        {
            _rewardStaticData = rewardStaticData;
            _amount = rewardAmount;
        }

        public int Id => _rewardStaticData.Id;

        public int Amount => 
            _amount;

        public Sprite IconSprite =>
            _rewardStaticData.Sprite;
    }
}