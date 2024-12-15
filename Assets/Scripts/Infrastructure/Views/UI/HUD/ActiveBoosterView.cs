using Infrastructure.Models.GameEntities.Boosters;
using Infrastructure.Views.UI.Loaders;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.HUD
{
    public class ActiveBoosterView : MonoBehaviour
    {
        [SerializeField] private IconLoader _iconLoader;
        [SerializeField] private TextMeshProUGUI _boostValueTextField;
        
        private BoosterModel _boosterModel;

        public void SetModel(BoosterModel boosterModel) =>
            _boosterModel = boosterModel;

        public void Draw()
        {
            if (_boosterModel != null)
            {
                _iconLoader.SetIconSprite(_boosterModel.IconSprite);
                _boostValueTextField.text = "x" + _boosterModel.BoostValue;
            }
        }
    }
}