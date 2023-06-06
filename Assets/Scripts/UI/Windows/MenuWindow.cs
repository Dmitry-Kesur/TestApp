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

            playButton.SetButtonText("Play Game");
            settingsButton.SetButtonText("Settings");
            
            scoreMultiplierText.text = $"Score Multiplier: {_menuWindowModel?.gameScoreMultiplier}";
        }

        public override void OnWindowShow()
        {
            base.OnWindowShow();
            playButton.OnButtonClickAction = OnPlayButtonClickHandler;
            settingsButton.OnButtonClickAction = OnSettingsButtonClickHandler;
            
            scoreText.text = $"Game Score: {_menuWindowModel?.gameScore}";
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
            playButton.OnButtonClickAction = null;
            settingsButton = null;
        }
    }
}