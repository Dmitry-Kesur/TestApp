using System;
using System.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    public class MultiplierData
    {
        public float multiplier { get; set; }
    }

    public class GameScoreModel
    {
        public Action<int> SaveGameScoreAction;
        private float _scoreMultiplier;
        private int _gameScore;

        public GameScoreModel(int gameGameScore)
        {
            _gameScore = gameGameScore;
        }

        public int GameScore => _gameScore;

        public float scoreMultiplier => _scoreMultiplier;

        public async Task InitScoreMultiplier()
        {
            var result = await Api.Get<MultiplierData>("https://mocki.io/v1/9d718cd5-3e36-48c2-9d46-4f043eecb646");
            _scoreMultiplier = result.multiplier;
        }

        public void SetScore(int scoreAmount)
        {
            _gameScore += Mathf.RoundToInt(scoreAmount * _scoreMultiplier);
            SaveGameScoreAction?.Invoke(_gameScore);
        }
    }
}