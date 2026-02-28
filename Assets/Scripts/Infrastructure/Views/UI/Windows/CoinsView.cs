using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class CoinsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinsTextField;

        public void UpdateCoins(int coinsAmount) =>
            _coinsTextField.text = coinsAmount.ToString();
    }
}