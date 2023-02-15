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
        [SerializeField] private List<Vector3> itemSpawnPoints;
        [SerializeField] private RectTransform itemsContainerTransform;
        [SerializeField] private RectTransform catchAreaTransform;
        [SerializeField] private RectTransform failAreaTransform;
        [SerializeField] private BaseButton settingsButton;
        [SerializeField] private TextMeshProUGUI caughtItemsText;
        [SerializeField] private TextMeshProUGUI failItemsText;
        [SerializeField] private SessionResultView sessionResultView;

        private const float maxSpawnDelay = 0.4f;
        private const int maxFailItems = 3;
        private const float defaultSpawnDelay = 2.2f;
        private float _spawnDelay;
        private int _caughtItemsAmount;
        private int _failItemsAmount;
        private List<ItemView> _dropItems;
        private Coroutine _spawnCoroutine;
        private GameSessionModel _gameSessionModel;

        public void Init(GameSessionModel gameSessionModel)
        {
            _dropItems = new List<ItemView>();
            _gameSessionModel = gameSessionModel;

            ResetSpawnDelay();

            settingsButton.button.onClick.AddListener(OnSettingsButtonClickHandler);
            sessionResultView.OnRetrySessionAction = OnRetrySessionHandler;
        }

        private void OnRetrySessionHandler()
        {
            sessionResultView.Hide();
            StartSpawnItems();
        }

        private void ResetSpawnDelay()
        {
            _spawnDelay = defaultSpawnDelay;
        }

        private void UpdateTexts()
        {
            caughtItemsText.text = $"Caught Items: {_caughtItemsAmount}";
            failItemsText.text = $"Fail Items: {_failItemsAmount}";
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
                dropItem.StartRotation();
                dropItem.isInteractable = false;
                dropItem.OnClickItemAction = () => { OnCatchItem(dropItem); };

                var spawnPoint = GetRandomSpawnPoint();

                Transform dropItemTransform;
                (dropItemTransform = dropItem.transform).SetParent(itemsContainerTransform, false);

                dropItemTransform.localPosition = new Vector3(spawnPoint.x, itemsContainerTransform.rect.yMax);

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
            if (IsFallsInCatchArea(itemView))
            {
                itemView.isInteractable = true;
            }

            if (IsFallsInFailArea(itemView) && !itemView.isCaught)
            {
                itemView.isInteractable = false;
                OnFailItem(itemView);
            }
        }

        private bool IsFallsInCatchArea(ItemView itemView)
        {
            var itemPositionY = itemView.transform.localPosition.y;
            return itemPositionY < catchAreaTransform.localPosition.y &&
                   itemPositionY > failAreaTransform.localPosition.y;
        }

        private bool IsFallsInFailArea(ItemView itemView) =>
            itemView.transform.localPosition.y < failAreaTransform.localPosition.y;

        private void OnCatchItem(ItemView itemView)
        {
            _caughtItemsAmount++;
            UpdateTexts();
            UpdateSpawnDelay();

            itemView.isCaught = true;
            itemView.AnimateAlpha(0).OnComplete(() =>
            {
                itemView.StopRotation();
                RemoveItem(itemView);
            });
        }

        private void UpdateSpawnDelay()
        {
            if (_spawnDelay < maxSpawnDelay) return;
            _spawnDelay -= 0.05f;
        }

        private void OnFailItem(ItemView itemView)
        {
            RemoveItem(itemView);
            _failItemsAmount++;
            UpdateTexts();

            if (_failItemsAmount == maxFailItems)
            {
                StopGameSession();

                sessionResultView.SetTotalResultText(_caughtItemsAmount, _failItemsAmount);
                sessionResultView.Show();
            }
        }

        private void StopGameSession()
        {
            _caughtItemsAmount = 0;
            _failItemsAmount = 0;

            UpdateTexts();

            StopCoroutine(_spawnCoroutine);
            RemoveItems();
            ResetSpawnDelay();
        }

        private void RemoveItem(ItemView itemView)
        {
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

        private Vector3 GetRandomSpawnPoint()
        {
            var randomIndex = Random.Range(0, itemSpawnPoints.Count);
            return itemSpawnPoints[randomIndex];
        }

        protected override void AfterHide()
        {
            base.AfterHide();
            settingsButton.button.onClick.RemoveListener(OnSettingsButtonClickHandler);
            sessionResultView.OnRetrySessionAction = null;
        }
    }
}