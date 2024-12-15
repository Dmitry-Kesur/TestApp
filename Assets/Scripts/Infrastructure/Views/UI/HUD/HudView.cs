using DG.Tweening;
using Infrastructure.Models.UI.HUD;
using Infrastructure.Views.UI.Buttons;
using TMPro;
using UnityEngine;

namespace Infrastructure.Views.UI.HUD
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private BaseButton _pauseGameButton;
        [SerializeField] private FailItemsView _failItemsView;
        [SerializeField] private TextMeshProUGUI _totalLevelScoreTextField;
        [SerializeField] private ActiveBoosterView _activeBoosterView;

        private Sequence _scoreAnimationSequence;
        private HudModel _hudModel;

        public void SetModel(HudModel hudModel)
        {
            _hudModel = hudModel;

            _activeBoosterView.SetModel(_hudModel.ActiveBoosterModel);
            _pauseGameButton.OnButtonClickAction = _hudModel.OnPauseGameButtonClick;
        }

        private void UpdateScore(int levelScore)
        {
            _scoreAnimationSequence?.Kill();
            _scoreAnimationSequence = DOTween.Sequence()
                .AppendCallback(() => { _totalLevelScoreTextField.text = levelScore.ToString(); })
                .Join(_totalLevelScoreTextField.transform.DOScale(1.2f, 0.5f))
                .Append(_totalLevelScoreTextField.transform.DOScale(1, 0.5f));
        }

        private void UpdateFailItems(int failItemsAmount)
        {
            _failItemsView.UpdateFailItems(failItemsAmount);
        }

        public void Clear()
        {
            UpdateScore(0);
            UpdateFailItems(0);

            _pauseGameButton.OnButtonClickAction = null;
        }

        public void UpdateLevelProgress()
        {
            UpdateScore(_hudModel.ActiveLevelScore);
            UpdateFailItems(_hudModel.ActiveLevelFailItems);
        }

        public void UpdateActiveBooster()
        {
            if (_hudModel.ActiveBoosterModel == null)
            {
                _activeBoosterView.gameObject.SetActive(false);
                return;
            }
            
            _activeBoosterView.gameObject.SetActive(true);
            _activeBoosterView.Draw();
        }

        public void Draw()
        {
            UpdateActiveBooster();
        }
    }
}