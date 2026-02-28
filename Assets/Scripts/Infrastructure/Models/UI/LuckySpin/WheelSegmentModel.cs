using Infrastructure.Data.LuckySpin;
using UnityEngine;

namespace Infrastructure.Models.UI
{
    public class WheelSegmentModel
    {
        private readonly WheelSegmentData _segmentData;

        public WheelSegmentModel(WheelSegmentData segmentData)
        {
            _segmentData = segmentData;
        }
        
        public int Index => _segmentData.Index;
        
        public int Weight => _segmentData.Weight;

        public int RewardId => _segmentData.RewardId;

        public int RewardAmount => _segmentData.RewardAmount;
        
        public Sprite IconSprite { get; set; }
    }
}