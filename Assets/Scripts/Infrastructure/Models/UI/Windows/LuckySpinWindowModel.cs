using System;
using System.Collections.Generic;

namespace Infrastructure.Models.UI.Windows
{
    public class LuckySpinWindowModel : BaseWindowModel
    {
        public Action OnCloseButtonClickAction;
        public Action StartFreeSpinAction;
        public Action StartSpinForRewardedAdAction;
        public Action<int> OnStartSpinAction;
        public Action<int> OnCompleteSpinAction;
        public Action RefreshAction;

        public bool CanFreeSpin { get; set; }
        
        public List<WheelSegmentModel> WheelSegments { get; set; }

        public void OnCloseButtonClicked() =>
            OnCloseButtonClickAction?.Invoke();

        public void OnStartFreeSpinButtonClicked() =>
            StartFreeSpinAction?.Invoke();

        public void OnStartSpinForRewardedAdButtonClicked() =>
            StartSpinForRewardedAdAction?.Invoke();

        public void OnCompleteSpin(int segmentIndex) =>
            OnCompleteSpinAction?.Invoke(segmentIndex);

        public void Refresh() =>
            RefreshAction?.Invoke();
    }
}