using System;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Notifications
{
    public class OverlayView : MonoBehaviour
    {
        [SerializeField] private Button _overlayButton;

        public Action OverlayButtonClick;

        private void Awake()
        {
            _overlayButton.onClick.RemoveAllListeners();
            _overlayButton.onClick.AddListener(OnOverlayButtonClick);
        }

        private void OnOverlayButtonClick()
        {
            OverlayButtonClick?.Invoke();
        }
    }
}