using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using TMPro;
using UI.Buttons;
using UnityEngine;

namespace UI
{
    public class GameSessionView : BaseView
    {
        [SerializeField] private RectTransform itemsContainerTransform;
        [SerializeField] private RectTransform catchAreaTransform;
        [SerializeField] private RectTransform failAreaTransform;
        [SerializeField] private BaseButton settingsButton;
        [SerializeField] private TextMeshProUGUI catchItemsText;
        [SerializeField] private TextMeshProUGUI failItemsText;
        [SerializeField] private SessionResultView sessionResultView;
        
        private float _spawnDelay;
        private const float spawnDecreaseAmount = 0.03f;
        private List<ItemView> _dropItems;
        private IEnumerator _spawnCoroutine;
        private GameSessionModel _gameSessionModel;

        public void Init(GameSessionModel gameSessionModel)
        {
            _dropItems = new List<ItemView>();
            _gameSessionModel = gameSessionModel;
        }

        protected override void OnShow()
        {
            settingsButton.OnButtonClickAction = OnSettingsButtonClickHandler;
            sessionResultView.OnRetrySessionAction = OnRetrySessionHandler;
            
            if (_gameSessionModel.isPause)
            {
                ResumeGameSession();
            }
            else
            {
                ResetGameSession();
            }
            
            SpawnItems();
        }
        
        private void OnRetrySessionHandler()
        {
            sessionResultView.Hide();
            SpawnItems();
        }

        private void ResetSpawnDelay()
        {
            _spawnDelay = _gameSessionModel.defaultSpawnDelay;
        }

        private void UpdateTexts()
        {
            catchItemsText.text = $"Catch Items: {_gameSessionModel.catchItemsAmount}";
            failItemsText.text = $"Fail Items: {_gameSessionModel.failItemsAmount}";
        }

        private void OnSettingsButtonClickHandler()
        {
            PauseGameSession();
            _gameSessionModel.SettingsButtonClick();
        }

        private void PauseGameSession()
        {
            _gameSessionModel.isPause = true;
            RemoveItems();
        }
        
        private void ResumeGameSession()
        {
            _gameSessionModel.isPause = false;
        }

        private void SpawnItems()
        {
            var itemModel = _gameSessionModel.GetGameItem();
            _spawnCoroutine = SpawnItem(itemModel);
            StartCoroutine(_spawnCoroutine);
        }

        private IEnumerator SpawnItem(ItemModel itemModel)
        {
            while (!_gameSessionModel.isPause)
            {
                var dropItem = itemModel.RenderItem();
                dropItem.interactable = false;
                dropItem.OnClickItemAction = () => { OnCatchItem(dropItem); };
                dropItem.StartRotation();

                Transform dropItemTransform;
                (dropItemTransform = dropItem.transform).SetParent(itemsContainerTransform, false);

                dropItemTransform.localPosition = new Vector3(GetRandomPositionX(), itemsContainerTransform.rect.yMax);
                
                StartMoveItem(dropItem);

                _dropItems.Add(dropItem);

                yield return new WaitForSeconds(_spawnDelay);
            }
        }

        private void StartMoveItem(ItemView itemView)
        {
            var endPosition = new Vector2(itemView.transform.position.x, itemsContainerTransform.anchorMax.y);
            var tween = itemView.MoveToPosition(endPosition);
            tween.OnUpdate(() => OnUpdateItemPosition(itemView));
        }

        private void OnUpdateItemPosition(ItemView itemView)
        {
            if (IsFallsCatchArea(itemView))
            {
                itemView.interactable = true;
            }

            if (IsFallsFailArea(itemView) && !itemView.isCatch)
            {
                itemView.interactable = false;
                OnFailItem(itemView);
            }
        }

        private bool IsFallsCatchArea(ItemView itemView)
        {
            var itemPositionY = itemView.transform.localPosition.y;
            return itemPositionY < catchAreaTransform.localPosition.y &&
                   itemPositionY > failAreaTransform.localPosition.y;
        }

        private bool IsFallsFailArea(ItemView itemView) =>
            itemView.transform.localPosition.y < failAreaTransform.localPosition.y;

        private void OnCatchItem(ItemView itemView)
        {
            _gameSessionModel.IncreaseCatchItemsAmount();
            UpdateTexts();
            UpdateSpawnDelay();

            itemView.isCatch = true;
            var tween = itemView.AnimateAlpha(0);
            tween.OnComplete(() => RemoveItem(itemView));
        }

        private void UpdateSpawnDelay()
        {
            if (_spawnDelay < _gameSessionModel.maxSpawnDelay) return;
            _spawnDelay -= spawnDecreaseAmount;
        }

        private void OnFailItem(ItemView itemView)
        {
            _gameSessionModel.IncreaseFailItemsAmount();
            
            RemoveItem(itemView);
            UpdateTexts();
            UpdateSpawnDelay();

            if (_gameSessionModel.IsFailItemsLimitReached())
            {
                sessionResultView.SetTotalResult(_gameSessionModel.catchItemsAmount, _gameSessionModel.failItemsAmount);
                sessionResultView.Show();
                
                ResetGameSession();
            }
        }

        private void ResetGameSession()
        {
            _gameSessionModel.Reset();
            UpdateTexts();
            RemoveItems();
            ResetSpawnDelay();

            if (_spawnCoroutine != null)
            {
                StopCoroutine(_spawnCoroutine);
            }
        }

        private void RemoveItem(ItemView itemView)
        {
            itemView.Clear();
            _dropItems.Remove(itemView);
            Destroy(itemView.gameObject);
        }

        private void RemoveItems()
        {
            for (int i = 0; i < _dropItems.Count; i++)
            {
                var item = _dropItems[i];
                RemoveItem(item);
            }
        }

        private float GetRandomPositionX()
        {
            var itemsContainerRect = itemsContainerTransform.rect;
            var randomPositionX = Random.Range(itemsContainerRect.center.x - itemsContainerRect.xMax, itemsContainerRect.center.x + itemsContainerRect.xMax);
            return randomPositionX;
        }

        protected override void OnHide()
        {
            base.OnHide();

            settingsButton.OnButtonClickAction = null;
            sessionResultView.OnRetrySessionAction = null;
        }
    }
}