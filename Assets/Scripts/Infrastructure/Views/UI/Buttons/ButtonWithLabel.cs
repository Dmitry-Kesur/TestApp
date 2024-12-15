using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Buttons
{
    public class ButtonWithLabel : BaseButton
    {
        [SerializeField] private TextMeshProUGUI buttonText;
        
        public void SetButtonText(string text)
        {
            buttonText.text = text;
        }
    }
}