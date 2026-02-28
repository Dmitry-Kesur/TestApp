using System;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Buttons
{
    public class BaseButton : MonoBehaviour
    {
        public Action OnButtonClickAction;
        
        [SerializeField] private Button button;

        public bool Interactable
        {
            get => button.interactable;
            set => button.interactable = value;
        }

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