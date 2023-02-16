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
        [SerializeField] private TextMeshProUGUI caughtItemsText;
        [SerializeField] private TextMeshProUGUI failItemsText;
        [SerializeField] private SessionResultView sessionResultView;
        
        private float _spawnDelay;
        private const float spawnDecreaseAmount = 0.03f;
        private List<ItemView> _dropItems;
        private Coroutine _spawnCoroutine;
        private GameSessionModel _gameSessionModel;

        public void Init(GameSessionModel gameSessionModel)
        {
            _dropItems = new List<ItemView>();
            _gameSessionModel = gameSessionModel;

            settingsButton.button.onClick.AddListener(OnSettingsButtonClickHandler);
            sessionResultView.OnRetrySessionAction = OnRetrySessionHandler;
            
            ResetSpawnDelay();
        }

        private void OnRetrySessionHandler()
        {
            sessionResultView.Hide();
            StartSpawnItems();
        }

        private void ResetSpawnDelay()
        {
            _spawnDelay = _gameSessionModel.defaultSpawnDelay;
        }

        private void UpdateTexts()
        {
            caughtItemsText.text = $"Caught Items: {_gameSessionModel.caughtItemsAmount}";
            failItemsText.text = $"Fail Items: {_gameSessionModel.failItemsAmount}";
        }

        private void OnSettingsButtonClickHandler()
        {
            StopGameSession();
            _gameSessionModel.SettingsButtonClick();
        }

        protected override void AfterShow()
        {
            UpdateTexts();
            StartSpawnItems();
        }

        private void StartSpawnItems()
        {
            var itemModel = _gameSessionModel.GetItem();
            _spawnCoroutine = StartCoroutine(SpawnItem(itemModel));
        }

        private IEnumerator SpawnItem(ItemModel itemModel)
        {
            while (true)
            {
                var dropItem = itemModel.RenderItem();
                dropItem.isInteractable = false;
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
                itemView.isInteractable = true;
            }

            if (IsFallsFailArea(itemView) && !itemView.isCaught)
            {
                itemView.isInteractable = false;
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
            _gameSessionModel.IncreaseCaughtItemsAmount();
            UpdateTexts();
            UpdateSpawnDelay();

            itemView.isCaught = true;
            itemView.AnimateAlpha(0).OnComplete(() => RemoveItem(itemView));
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

            if (_gameSessionModel.IsFailItemsLimitReached())
            {
                sessionResultView.SetTotalResultText(_gameSessionModel.caughtItemsAmount, _gameSessionModel.failItemsAmount);
                sessionResultView.Show();
                
                StopGameSession();
            }
        }

        private void StopGameSession()
        {
            _gameSessionModel.OnStopGameSession();
            
            UpdateTexts();
            StopCoroutine(_spawnCoroutine);
            RemoveItems();
            ResetSpawnDelay();
        }

        private void RemoveItem(ItemView itemView)
        {
            itemView.StopRotation();
            _dropItems.Remove(itemView);
            Destroy(itemView.gameObject);
        }

        private void RemoveItems()
        {
            foreach (var item in _dropItems)
            {
                Destroy(item.gameObject);
            }

            _dropItems.Clear();
        }

        private float GetRandomPositionX()
        {
            var itemsContainerRect = itemsContainerTransform.rect;
            var randomPositionX = Random.Range(itemsContainerRect.center.x - itemsContainerRect.xMax, itemsContainerRect.center.x + itemsContainerRect.xMax);
            return randomPositionX;
        }

        protected override void AfterHide()
        {
            base.AfterHide();
            settingsButton.button.onClick.RemoveListener(OnSettingsButtonClickHandler);
            sessionResultView.OnRetrySessionAction = null;
        }
    }
}