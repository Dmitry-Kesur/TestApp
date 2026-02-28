using System.Collections.Generic;
using Infrastructure.Models.GameEntities.DailyBonus;

namespace Infrastructure.Models.UI.Windows
{
    public class DailyBonusWindowModel : BaseWindowModel
    {
        public IReadOnlyList<DailyBonusDayModel> DayModels { get; set; }
    }
}