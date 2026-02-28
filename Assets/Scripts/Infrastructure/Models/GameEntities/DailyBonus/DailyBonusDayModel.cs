using System;
using Infrastructure.Data.DailyBonus;
using Infrastructure.Models.UI.Items;
using UnityEngine;

namespace Infrastructure.Models.GameEntities.DailyBonus
{
    public class DailyBonusDayModel : IDrawableModel
    {
        public event Action<DailyBonusDayModel> OnTakeRewardAction;
        
        private DailyBonusDayData _dayData;

        public void SetData(DailyBonusDayData dayData) =>
            _dayData = dayData;

        public int Day => _dayData.Day;

        public int RewardResourceId => _dayData.RewardResourceId;
        
        public int RewardAmount => _dayData.RewardAmount;

        public bool CanTakeReward { get; set; }

        public Sprite IconSprite { get; set; }

        public void TakeReward() =>
            OnTakeRewardAction?.Invoke(this);
    }
}