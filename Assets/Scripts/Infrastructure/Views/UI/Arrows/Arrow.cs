using System;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Arrows
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private Button arrowButton;
        public Action OnArrowClickAction;

        private void Awake()
        {
            arrowButton.onClick.AddListener(OnArrowClickHandler);
        }

        private void OnArrowClickHandler()
        {
            OnArrowClickAction?.Invoke();
        }

        private void OnDisable()
        {
            arrowButton.onClick.RemoveListener(OnArrowClickHandler);
        }
    }
}