using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class BaseButton : MonoBehaviour
    {
        public Action OnButtonClickAction;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI buttonText;

        private void Awake()
        {
            button.onClick.RemoveAllListeners();
            button.onClick?.AddListener(OnButtonClickHandler);
        }

        private void OnButtonClickHandler()
        {
            OnButtonClickAction?.Invoke();
        }

        public void SetButtonText(string text)
        {
            buttonText.text = text;
        }
    }
}