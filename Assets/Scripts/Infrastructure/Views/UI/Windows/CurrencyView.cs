using Infrastructure.Models.GameEntities;
using Infrastructure.Models.GameEntities.Currency;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.Windows
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _currencyAmountTextField;

        private CurrencyModel _currencyModel;

        public void Init(CurrencyModel currencyModel)
        {
            _currencyModel = currencyModel;
            _currencyModel.OnUpdateCurrencyAction += UpdateCurrency;
            UpdateCurrency();
        }
        
        private void UpdateCurrency() =>
            _currencyAmountTextField.text = _currencyModel.CurrencyAmount.ToString();

        public void Clear()
        {
            _currencyModel.OnUpdateCurrencyAction -= UpdateCurrency;
        }
    }
}