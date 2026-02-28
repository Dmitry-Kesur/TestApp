using Infrastructure.Views.UI.Loaders;
using TMPro;
using UnityEngine;

namespace Infrastructure.Models.UI
{
    public class WheelSegmentView : MonoBehaviour
    {
        [SerializeField] private IconLoader _icon;
        [SerializeField] private TextMeshProUGUI _textField;

        private WheelSegmentModel _model;

        public void SetModel(WheelSegmentModel model) =>
            _model = model;
        
        public void Draw()
        {
            _icon.SetIconSprite(_model.IconSprite);
            _textField.text = _model.RewardAmount.ToString();
        }
    }
}