using System;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Buttons
{
    public class BaseButton : MonoBehaviour
    {
        public Action OnButtonClickAction;
        
        [SerializeField] private Button button;

        private void OnEnable()
        {
            button.onClick.AddListener(OnButtonClickHandler);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnButtonClickHandler);
        }

        private void OnButtonClickHandler()
        {
            OnButtonClickAction?.Invoke();
        }
    }
}