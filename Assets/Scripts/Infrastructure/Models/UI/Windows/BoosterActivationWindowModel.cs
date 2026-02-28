using System;
using System.Collections.Generic;
using Infrastructure.Models.GameEntities.Boosters;

namespace Infrastructure.Models.UI.Windows
{
    public class BoosterActivationWindowModel : BaseWindowModel
    {
        public Action CancelAction;
        
        public IReadOnlyList<BoosterModel> GetBoosterModels { get; set; }

        public void OnCancelButtonClicked() =>
            CancelAction?.Invoke();
    }
}