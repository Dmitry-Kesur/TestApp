using Infrastructure.Data.LuckySpin;
using Infrastructure.Models.UI;

namespace Infrastructure.Factories
{
    public class LuckySpinFactory
    {
        public WheelSegmentModel CreateWheelSegmentModel(WheelSegmentData segmentData)
        {
            var segmentModel = new WheelSegmentModel(segmentData);
            return segmentModel;
        }
    }
}