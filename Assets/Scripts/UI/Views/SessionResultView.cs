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

        protected override void OnShow()
        {
            base.OnShow();
            retryButton.OnButtonClickAction = OnRetryButtonClickHandler;
        }

        public void SetTotalResult(int catchItemsAmount, int failItemsAmount)
        {
            totalResultText.text = $"Catch Items: {catchItemsAmount}\nFail Items: {failItemsAmount}";
        }

        private void OnRetryButtonClickHandler()
        {
            retryButton.OnButtonClickAction = null;
            OnRetrySessionAction?.Invoke();
        }
    }
}