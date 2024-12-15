using System;
using UnityEngine;

namespace Infrastructure.Views.UI.Buttons
{
    public class ToggleButton : MonoBehaviour
    {
        [SerializeField] private ButtonWithIcon _buttonWithIcon;
        [SerializeField] private Sprite _toggleOnSprite;
        [SerializeField] private Sprite _toggleOffSprite;

        public Action<bool> OnToggleStateChange;
        
        private bool _isToggleOn;

        private void OnEnable()
        {
            _buttonWithIcon.OnButtonClickAction = OnButtonClickHandler;
        }

        private void OnDisable()
        {
            _buttonWithIcon.OnButtonClickAction = null;
        }

        public void ChangeToggleState(bool toggleState)
        {
            _isToggleOn = toggleState;
            UpdateButtonIconByToggleState();
        }

        private void OnButtonClickHandler()
        {
            ChangeToggleButtonState();
            OnToggleStateChange?.Invoke(_isToggleOn);
        }

        private void ChangeToggleButtonState()
        {
            _isToggleOn = !_isToggleOn;

            UpdateButtonIconByToggleState();
        }

        private void UpdateButtonIconByToggleState()
        {
            var currentSprite = _isToggleOn ? _toggleOnSprite : _toggleOffSprite;
            _buttonWithIcon.SetSprite(currentSprite);
        }
    }
}