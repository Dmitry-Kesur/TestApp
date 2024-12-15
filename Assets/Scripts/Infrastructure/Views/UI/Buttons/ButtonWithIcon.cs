using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Views.UI.Buttons
{
    public class ButtonWithIcon : BaseButton
    {
        [SerializeField] private Image _icon;

        public void SetSprite(Sprite sprite)
        {
            _icon.sprite = sprite;
        }
    }
}