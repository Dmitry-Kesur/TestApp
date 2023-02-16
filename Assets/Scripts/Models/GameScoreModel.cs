using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameScoreModel
    {
        public Action OnChangeScoreAction;
        private float _scoreMultiplier;
        private int _gameScore;

        public GameScoreModel(int gameGameScore)
        {
            _gameScore = gameGameScore;
        }

        public int gameScore => _gameScore;

        public float scoreMultiplier => _scoreMultiplier;

        public void SetScore(int scoreAmount)
        {
            _gameScore += Mathf.RoundToInt(scoreAmount * _scoreMultiplier);
            OnChangeScoreAction?.Invoke();
        }

        public void SetScoreMultiplier(float scoreMultiplier)
        {
            _scoreMultiplier = scoreMultiplier;
        }
    }
}