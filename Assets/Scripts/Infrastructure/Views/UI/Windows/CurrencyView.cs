using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currencyAmountTextField;

        public void UpdateCurrency(int currencyAmount) =>
            _currencyAmountTextField.text = currencyAmount.ToString();
    }
}