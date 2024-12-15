using Infrastructure.Data.Notifications;
using Infrastructure.Views.UI.Buttons;
using UnityEngine;

namespace Infrastructure.Views.UI.Notifications
{
    public class NotificationWithAdsView : NotificationView
    {
        [SerializeField] private ButtonWithIcon _showAdsButton;

        public override void OnShowNotification(NotificationModel notificationModel)
        {
            base.OnShowNotification(notificationModel);
            _showAdsButton.OnButtonClickAction = OnShowAdsButtonClick;
        }

        private void OnShowAdsButtonClick()
        {
            ((NotificationWithAdsModel) NotificationModel).ShowAdsAction?.Invoke();
        }
    }
}