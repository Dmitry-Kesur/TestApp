using TMPro;
using UI;
using UI.Buttons;
using UnityEngine;

namespace DefaultNamespace
{
    public class MenuWindow : BaseWindow
    {
        [SerializeField] private BaseButton playButton;
        [SerializeField] private BaseButton settingsButton;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI scoreMultiplierText;
        private MenuWindowModel _menuWindowModel;
        
        public override void Init(BaseWindowModel windowModel)
        {
            _menuWindowModel = windowModel as MenuWindowModel;

            playButton.button.onClick.AddListener(OnPlayButtonClickHandler);
            settingsButton.button.onClick.AddListener(OnSettingsButtonClickHandler);
            
            playButton.SetButtonText("Play Game");
            settingsButton.SetButtonText("Settings");
            
            scoreText.text = $"Game Score: {_menuWindowModel?.gameScore}";
            scoreMultiplierText.text = $"Score Multiplier: {_menuWindowModel?.gameScoreMultiplier}";
        }

        private void OnPlayButtonClickHandler()
        {
            _menuWindowModel.PlayButtonClick();
        }
        
        private void OnSettingsButtonClickHandler()
        {
            _menuWindowModel.OnSettingsButtonClick();
        }

        public override void OnWindowHide()
        {
            base.OnWindowHide();
            playButton.button.onClick.RemoveListener(OnPlayButtonClickHandler);
            settingsButton.button.onClick.RemoveListener(OnSettingsButtonClickHandler);
        }
    }
}