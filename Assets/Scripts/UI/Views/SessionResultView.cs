using System;
using TMPro;
using UI.Buttons;
using UnityEngine;

namespace UI
{
    public class SessionResultView : BaseView
    {
        public Action OnRetrySessionAction;
        [SerializeField] private TextMeshProUGUI totalResultText;
        [SerializeField] private BaseButton retryButton;

        protected override void AfterShow()
        {
            base.AfterShow();
            retryButton.button.onClick.AddListener(OnRetryButtonClickHandler);
        }

        public void SetTotalResultText(int catchItemsAmount, int failItemsAmount)
        {
            totalResultText.text = $"Catch Items: {catchItemsAmount}\nFail Items: {failItemsAmount}";
        }

        private void OnRetryButtonClickHandler()
        {
            retryButton.button.onClick.RemoveListener(OnRetryButtonClickHandler);
            OnRetrySessionAction?.Invoke();
        }
    }
}