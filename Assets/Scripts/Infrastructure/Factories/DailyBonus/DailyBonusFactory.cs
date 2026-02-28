using Infrastructure.Data.DailyBonus;
using Infrastructure.Models.GameEntities.DailyBonus;

namespace Infrastructure.Factories
{
    public class DailyBonusFactory
    {
        public DailyBonusDayModel CreateDayModel(DailyBonusDayData dayData)
        {
            var dayModel = new DailyBonusDayModel();
            dayModel.SetData(dayData);
            return dayModel;
        }
    }
}