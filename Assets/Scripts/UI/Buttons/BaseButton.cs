using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class BaseButton : MonoBehaviour
    {
        [SerializeField] public Button button;
        [SerializeField] private TextMeshProUGUI buttonText;

        public void SetButtonText(string text)
        {
            buttonText.text = text;
        }
    }
}